#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2023-01-25.
// Comment: Authenticate administrator users.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginUser
{
    public class AuthenticateUser : IAuthenticate
    {
        private readonly IApplicationDbContext context;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IAuthenticateQuery authenticateQuery;
        private readonly ILogService logService;
        private readonly ICryptographyService cryptography;
        readonly AuthenticateDto authenticate;
        private readonly ISecurity security;
        /// <summary>
        /// Constructor that injects the interface token, database context and tokens querys.
        /// </summary>
        /// <param name="context">Context database</param>
        /// <param name="tokenGenerator">Inject the interface with the necessary methods to generate the token</param>
        /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
        /// <param name="authenticate">Contains the main values for user authentication.</param>
        /// <param name="security">Inject Security Process</param>
        public AuthenticateUser(IApplicationDbContext context, ITokenGenerator tokenGenerator, IAuthenticateQuery authenticateQuery, ILogService logService, UserDto authenticate, ICryptographyService cryptography, ISecurity security)
        {
            this.context = context;
            this.tokenGenerator = tokenGenerator;
            this.authenticateQuery = authenticateQuery;
            this.logService = logService;
            this.authenticate = authenticate;
            this.cryptography = cryptography;
            this.security = security;
        }

        /// <summary>
        /// Contains all the necessary business rules to authenticate a user.
        /// We obtain the main information, person id, operador id, time expiration and we call the function  that generates the token 
        /// and register the session in the database.  
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>AuthResponse object with the new token and refresh token</returns>
        public async Task<AuthResponse> Auth(CancellationToken cancellationToken)
        {
            try
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Information, GlobalConstantMessages.LOGINITAUTHENTICATEUSER, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
              
                UserDto userDto = (UserDto)authenticate;
                Operator? @operator = null;
                AuthResponse auth;
                if (authenticate.TypeAuth == GlobalConstantHelpers.USERXPOS && !userDto.IsInternal)
                {                   
                    auth = await GetAppExternaToken(userDto.Identification, cancellationToken);
                    if (!auth.Auth)
                    {
                        return new AuthResponse() { Auth = false };
                    }

                    string xmlUser = GenerateXml(userDto);
                    RequestExternalCommDto requestExternalComm = GenerateRequestUser(xmlUser);
                    ResponseExternalCommDto responseExternalComm = await Execute(requestExternalComm, auth.Token, cancellationToken);

                    if (!responseExternalComm.IsValid)
                    {
                        return new AuthResponse() { Auth = false };
                    }

                    TpeDoc? test = DeserializeXmlResponse(responseExternalComm);
                    if (test == null || (test.Response.WmCode.Value != 101 && test.Response.WmCode.Value != 004) || 
                        (test.Response.Validpass.Status != GlobalConstantHelpers.ACTIVE_OPERATORSTATUS && test.Response.Validpass.Status != GlobalConstantHelpers.ACTIVE_FOR_EXPIRING_OPERATORSTATUS))
                    {
                        return new AuthResponse() { Auth = false };
                    }
                    
                    @operator = await CreateOrUpdateOperator(userDto, userDto.StatusOperator, cancellationToken);
                }
                else
                {
                    @operator = await GetUserInternal(userDto, cancellationToken);
                }

                AuthOperatorStoreCommand command = new() { UserDto = userDto, Operator = @operator };
                auth =  await new AuthOperatorStoreCommandHandler(context, logService, authenticateQuery, tokenGenerator).Handle(command, cancellationToken);
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Information, GlobalConstantMessages.LOGENDTAUTHENTICATEUSER, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                return auth;
            }
            catch (CustomException ex)
            {
                 _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error,
                    string.Concat(GlobalConstantMessages.LOGERRORAUTHENTICATEUSER, ex.GetException()), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                throw;
            }
        }

        /// <summary>
        /// Method obtaing api key the internal app
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthResponse> GetAppExternaToken(string identification, CancellationToken cancellationToken)
        {
            ApiKey? externalApp = await context.API_KEY.AsNoTracking().Include(x => x.EXTERNAL_APPLICATION).OrderByDescending(x => x.EXPIRATION_TIME).FirstOrDefaultWithNoLockAsync(x => x.EXTERNAL_APPLICATION != null && x.EXTERNAL_APPLICATION.CODE == GlobalConstantHelpers.INTERNALAPPNAME && x.IS_ACTIVE, cancellationToken);
            if (externalApp == null)
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error, GlobalConstantMessages.LOGERRORAUTHENTICATEUSERXPOSGETAPIKEY, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                return new AuthResponse() { Auth = false };
            }

            if (externalApp.EXPIRATION_TIME < DateTime.UtcNow)
            {
                externalApp.EXPIRATION_TIME = externalApp.EXPIRATION_TIME.AddYears(1);
                await context.SaveChangesAsync(cancellationToken);
            }
            return await ProcessGenerateToken(externalApp.EXTERNAL_APPLICATION ?? new(), identification, cancellationToken);
        }

        /// <summary>
        /// Generate to xml with user informat data 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private string GenerateXml(UserDto userDto)
        {
            XmlDocument doc = new();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration(GlobalConstantHelpers.VERSIONXMLUSERDECLARATION, GlobalConstantHelpers.ENCODINGXML, null);
            XmlElement? root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement elementTpeDocument = doc.CreateElement(string.Empty, GlobalConstantHelpers.NAMEXMLUSERELEMENTMAIN, string.Empty);
            doc.AppendChild(elementTpeDocument);
            elementTpeDocument.SetAttribute(GlobalConstantHelpers.VERSIONXMLUSERATRIBUTENAME, GlobalConstantHelpers.VERSIONXMLUSERDECLARATION);
            elementTpeDocument.AppendChild(SetAttributeHeader(userDto, doc));
            elementTpeDocument.AppendChild(SetAttributeRequest(userDto, doc));
            return doc.InnerXml;
        }

        /// <summary>
        /// Genera data for request user web method
        /// </summary>
        /// <param name="xmlUser"></param>
        /// <returns></returns>
        private RequestExternalCommDto GenerateRequestUser(string xmlUser)
        {
            return new()
            {
                Service = Environment.GetEnvironmentVariable(GlobalConstantHelpers.CONFIGURATION_SERVICE_LOGIN_ID_WM) ?? throw new CustomException(GlobalConstantMessages.LOGERRORGETIDLOGINSERVICEWEBMETHOD, HttpStatusCode.UnprocessableEntity),
                Timeout = GlobalConstantHelpers.TIMEOUTUSERWEBMETHOD,
                PayloadType = GlobalConstantHelpers.PAYLOADTYPE,
                Payload = new PayloadDto()
                {                    
                    Method = GlobalConstantHelpers.EVENTMETHODPOSTADMINISTRATORS,
                    ContentType = GlobalConstantHelpers.CONTENTTYPE,
                    QueryParams = new List<QueryParamDto> { new QueryParamDto() { Key = GlobalConstantHelpers.KEYQUERYPARAM, Value = xmlUser } }
                }
            };
        }

        /// <summary>
        /// Set attribute values for xml header
        /// </summary>
        /// <param name="userDto">objet with parameter values</param>
        /// <param name="doc">documento with xmlElement</param>
        /// <returns></returns>
        private XmlElement SetAttributeHeader(UserDto userDto, XmlDocument doc)
        {           
            XmlElement elementHeader = doc.CreateElement(string.Empty, GlobalConstantHelpers.NAMEXMLUSERELEMENTHEADER, string.Empty);
            elementHeader.SetAttribute(GlobalConstantHelpers.APPLICATIONXMLUSERATRIBUTENAME, GlobalConstantHelpers.APPLICATIONXMLUSERATRIBUTEVALUE);
            elementHeader.SetAttribute(GlobalConstantHelpers.ENTITYXMLUSERATRIBUTENAME, GlobalConstantHelpers.ENTITYXMLUSERATRIBUTEVALUE);
            elementHeader.SetAttribute(GlobalConstantHelpers.OPERATIONXMLUSERATRIBUTENAME, GlobalConstantHelpers.OPERATIONXMLUSERATRIBUTEVALUE);
            elementHeader.SetAttribute(GlobalConstantHelpers.SOURCEXMLUSERATRIBUTENAME, GlobalConstantHelpers.SOURCEXMLUSERATRIBUTEVALUE);
            elementHeader.SetAttribute(GlobalConstantHelpers.FOLIOXMLUSERATRIBUTENAME, GlobalConstantHelpers.FOLIODEFAULTRATRIBUTEVALUE);
            elementHeader.SetAttribute(GlobalConstantHelpers.PLACEXMLUSERATRIBUTENAME, userDto.CrPlace);
            elementHeader.SetAttribute(GlobalConstantHelpers.STOREXMLUSERATRIBUTENAME, userDto.CrStore);
            elementHeader.SetAttribute(GlobalConstantHelpers.TILLXMLUSERATRIBUTENAME, userDto.Till);
            elementHeader.SetAttribute(GlobalConstantHelpers.ADMINISTRATIVEDATEVERSIONXMLUSERATRIBUTENAME, userDto.AdministrativeDate);
            elementHeader.SetAttribute(GlobalConstantHelpers.PROCESSDATEVERSIONXMLUSERATRIBUTENAME, userDto.ProcessDate);
            return elementHeader;
        }

        /// <summary>
        /// Set attribute values for xml request
        /// </summary>
        /// <param name="userDto">objet with parameter values</param>
        /// <param name="doc">documento with xmlElement</param>
        /// <returns></returns>
        private XmlElement SetAttributeRequest(UserDto userDto, XmlDocument doc)
        {            
            XmlElement elementRequest = doc.CreateElement(string.Empty, GlobalConstantHelpers.NAMEXMLUSERELEMENTREQUEST, string.Empty);
            XmlElement validpass = doc.CreateElement(string.Empty, GlobalConstantHelpers.NAMEXMLUSERELEMENTVALIDAPASS, string.Empty);
            elementRequest.AppendChild(validpass);
            validpass.SetAttribute(GlobalConstantHelpers.TYPEXMLUSERATRIBUTENAME, GlobalConstantHelpers.TYPEXMLUSERATRIBUTEVALUE);
            validpass.SetAttribute(GlobalConstantHelpers.IDUSUARIOXMLUSERATRIBUTENAME, userDto.User);
            validpass.SetAttribute(GlobalConstantHelpers.PASSWORDXMLUSERATRIBUTENAME, cryptography.Decrypt(userDto.Password));          
            validpass.SetAttribute(GlobalConstantHelpers.RETRYXMLUSERATRIBUTENAME, GlobalConstantHelpers.RETRYXMLUSERATRIBUTEVALUE);
            validpass.SetAttribute(GlobalConstantHelpers.APPXMLUSERATRIBUTENAME, string.IsNullOrWhiteSpace(userDto.Type) ? GlobalConstantHelpers.APPXMLUSERATRIBUTEVALUE : userDto.Type);
            validpass.SetAttribute(GlobalConstantHelpers.POSPKGXMLUSERATRIBUTENAME, string.Empty);
            return elementRequest;
        }

        /// <summary>
        /// Obtains the data operator internal
        /// If the user is from xpos but is internal, they must be previously registered in an initial load and therefore the password is encrypted.
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private async Task<Operator> GetUserInternal(UserDto userDto, CancellationToken cancellationToken)
        {
            if (authenticate.TypeAuth == GlobalConstantHelpers.USERXPOS)
            {
                userDto.Password = cryptography.Decrypt(userDto.Password);
                userDto.Password = cryptography.EncryptHash(userDto.Password);
            }
            Operator? @operator = await authenticateQuery.GetOperatorActiveForUserPassword(userDto.User, userDto.Password, cancellationToken);
            return @operator ?? throw new CustomException(GlobalConstantMessages.USERTOKENTIMEEXPIRATIONNOTFOUND, HttpStatusCode.UnprocessableEntity);
        }             

        /// <summary>
        /// Connect with generic web api to connect to WM
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<ResponseExternalCommDto> Execute(RequestExternalCommDto request, string token, CancellationToken cancellationToken)
        {
            ResponseExternalCommDto responseExternal = new() { IsValid = false };
            string dataJson = JsonConvert.SerializeObject(request);
            StringContent data = new(dataJson, Encoding.UTF8, GlobalConstantHelpers.MEDIATYPEJSON);

            var url = Environment.GetEnvironmentVariable(GlobalConstantHelpers.OXXO_CLOUD_EXTERNALCOMM_URL) ?? throw new CustomException(GlobalConstantMessages.LOGERRORGETURLLOGINSERVICEWEBMETHOD, HttpStatusCode.UnprocessableEntity);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(GlobalConstantHelpers.HEADER_AUTHORIZATION, token);

            HttpResponseMessage response;
            try
            {                
                response = await client.PostAsync(url, data, cancellationToken);
            }
            catch (Exception ex)
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error, $"{GlobalConstantMessages.LOGERRORWEBMETHODCONECTION} : {ex.GetException()}", GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                return responseExternal;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    ResponseExternalCommDto? responseObject = JsonConvert.DeserializeObject<ResponseExternalCommDto>(responseContent);
                    if (responseObject != null && !string.IsNullOrWhiteSpace(responseObject.Data))
                    {
                        return responseObject;
                    }
                    _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error, GlobalConstantMessages.LOGERRORWEBMETHODCONECTION, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                    return responseExternal;
                }
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error, GlobalConstantMessages.LOGERRORWEBMETHODCONECTION, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                return responseExternal;
            }
            _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Error, $"{GlobalConstantMessages.LOGERRORWEBMETHODCONECTION}, error code: {response.StatusCode}", GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
            return responseExternal;
        }

        /// <summary>
        /// Deserialize xml object
        /// </summary>
        /// <param name="responseExternalComm"></param>
        /// <returns></returns>
        private TpeDoc? DeserializeXmlResponse(ResponseExternalCommDto responseExternalComm)
        {
            StringReader reader;
            XmlSerializer serializer = new(typeof(TpeDoc));
            reader = new(responseExternalComm.Data);
            return (TpeDoc?)serializer.Deserialize(reader);
        }

        private async Task<Operator> CreateOrUpdateOperator(UserDto userDto, string status, CancellationToken cancellationToken)
        {
            Operator? @operator = await authenticateQuery.GetOperatorActiveForUser(userDto.User, cancellationToken);
            OperatorStatus statusOperador = await authenticateQuery.GetOperadtorStatus(status, cancellationToken);
            Workgroup workgroupUser = await context.WORKGROUP.FirstAsync(x => x.CODE == userDto.WorkGroup, cancellationToken: cancellationToken);
            string[] fullNameSplit = userDto.FullName.Split(GlobalConstantHelpers.CARACTERPAID);
            if (@operator == null)
            {
                DateTime dtFecha = DateTime.UtcNow;                
                using var transaction = context.database.BeginTransaction();
                try
                {
                   
                    Person person = new()
                    {
                        NAME = fullNameSplit.AsEnumerable().First(),
                        MIDDLE_NAME = fullNameSplit.Length == 4 ? fullNameSplit[1] : null,
                        LASTNAME_PAT = fullNameSplit[^2],
                        LASTNAME_MAT = fullNameSplit.AsEnumerable().Last(),
                        IS_ACTIVE = true,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                        CREATED_DATETIME = dtFecha,
                        OPERATOR = new()
                        {
                            OPERATOR_STATUS_ID = statusOperador.OPERATOR_STATUS_ID,
                            USER_NAME = userDto.User,
                            FL_INTRN = userDto.IsInternal,
                            IS_ACTIVE = true,
                            LCOUNT = 0,
                            CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                            CREATED_DATETIME = dtFecha
                        }
                    };
                    await context.PERSON.AddAsync(person, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    @operator = person.OPERATOR;                    

                    UserWorkgroupLink userWorkgroupLink = new()
                    {
                        GUID = @operator.OPERATOR_ID,
                        WORKGROUP_ID = workgroupUser.WORKGROUP_ID,
                        IS_ACTIVE = true,
                        LCOUNT = 0,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                        CREATED_DATETIME = dtFecha
                    };
                    await context.USER_WORKGROUP_LINK.AddAsync(userWorkgroupLink, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();                   
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);                  
                    throw;
                }                
            }
            else
            {
                if (@operator.PERSON.NAME == userDto.User)
                {
                    @operator.PERSON.NAME = fullNameSplit.AsEnumerable().First();
                    @operator.PERSON.MIDDLE_NAME = fullNameSplit.Length == 4 ? fullNameSplit[1] : null;
                    @operator.PERSON.LASTNAME_PAT = fullNameSplit[^2];
                    @operator.PERSON.LASTNAME_MAT = fullNameSplit.AsEnumerable().Last();
                    @operator.PERSON.IS_ACTIVE = true;
                    @operator.PERSON.LCOUNT += 1;
                    @operator.PERSON.MODIFIED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification);
                    @operator.PERSON.MODIFIED_DATETIME = DateTime.UtcNow;
                }              

                @operator.OPERATOR_STATUS_ID = statusOperador.OPERATOR_STATUS_ID;
                @operator.MODIFIED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification);
                @operator.LCOUNT += 1;
                @operator.MODIFIED_DATETIME = DateTime.UtcNow;

                UserWorkgroupLink userWorkgroupLink = await context.USER_WORKGROUP_LINK.FirstAsync(x => x.GUID == @operator.OPERATOR_ID && x.IS_ACTIVE, cancellationToken: cancellationToken);
                userWorkgroupLink.WORKGROUP_ID = workgroupUser.WORKGROUP_ID;
                userWorkgroupLink.IS_ACTIVE = true;
                userWorkgroupLink.LCOUNT += 1;
                userWorkgroupLink.MODIFIED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification);
                userWorkgroupLink.MODIFIED_DATETIME = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
            }           
            return @operator;
        }

        /// <summary>
        /// Contains all bussines rules to generate token
        /// </summary>
        /// <param name="externalApplication"></param>
        /// <param name="identification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AuthResponse> ProcessGenerateToken(ExternalApplication externalApplication, string identification, CancellationToken cancellationToken)
        {
            var auhtResponse = await ValidateProcessExistAndRegresToken(externalApplication.EXTERNAL_APPLICATION_ID, authenticate.Identification, cancellationToken);
            if (auhtResponse != null)
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Information, GlobalConstantMessages.LOGINITAUTHENTICATEUSER, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
                return auhtResponse;
            }
            AuthResponse authResponse = await RegisterToken(externalApplication, identification, cancellationToken);
            _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEUSER, LogTypeEnum.Information, GlobalConstantMessages.LOGENDTAUTHENTICATEUSER, GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEUSER);
            return authResponse;
        }          

        /// <summary>
        /// Contains all the business rules needed to create a token and register it in the database.
        /// </summary>
        /// <param name="externalApplication">Object operator with values for create token</param>
        /// <param name="identification">Value id opertaor default</param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AuthResponse> RegisterToken(ExternalApplication externalApplication, string identification, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwtToken = tokenGenerator.GenerateJWTToken(externalApplication.NAME, externalApplication.CODE, externalApplication.TIME_EXPIRATION_TOKEN.ToString(), externalApplication.EXTERNAL_APPLICATION_ID.ToString());
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            string newRefreshToken = tokenGenerator.GenerateRefreshToken();

            var entity = new SessionToken()
            {
                GUID = externalApplication.EXTERNAL_APPLICATION_ID,
                SESSION_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
                TOKEN = token,
                REFRESH_TOKEN = newRefreshToken,
                EXPIRATION_TOKEN = jwtToken.ValidTo,
                START_DATETIME = DateTime.UtcNow,
                END_DATETIME = DateTime.MinValue,
                IS_ACTIVE = true,
                LCOUNT = 0,
                CREATED_BY_OPERATOR_ID = Guid.Parse(identification),
                CREATED_DATETIME = DateTime.UtcNow
            };
            await context.SESSION_TOKEN.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return new AuthResponse
            {
                Auth = true,
                Token = token,
                RefreshToken = newRefreshToken,
                Exp = jwtToken.ValidTo
            };
        }

        /// <summary>
        /// Validates if there is an active session, then checks the validity of the token, if it expires it calls the method that contains the business rules to update the token.
        /// </summary>
        /// <param name="nameDevice">Contains the name value for user authentication.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="source">Contains the source value for log.</param>
        /// <returns></returns>
        private async Task<AuthResponse?> ValidateProcessExistAndRegresToken(Guid operadorId, string identification, CancellationToken cancellationToken)
        {
            if (await authenticateQuery.ValidateSessionActive(operadorId))
            {
                var auhtResponse = await authenticateQuery.GetSessionToken(operadorId);
                if (auhtResponse.Exp != null && await authenticateQuery.ValidateExpirationToken(auhtResponse.Token, cancellationToken))
                {
                    return auhtResponse;
                }
                else
                {
                    return await UpdateToken(auhtResponse, identification, cancellationToken);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the values sent by parameter generating an object of type tokenCommandHandler and calls the event that updates the token.
        /// </summary>
        /// <param name="auhtResponse">Contains the object with data token</param>
        /// <param name="cancellationToken"></param>
        /// <param name="source">Contains the source value for log.</param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private async Task<AuthResponse> UpdateToken(AuthResponse auhtResponse, string identification, CancellationToken cancellationToken)
        {
            try
            {
                RefreshTokenCommand command = new() { BearerToken = auhtResponse.Token, RefreshToken = auhtResponse.RefreshToken, Identification = identification };
                return await new TokenCommandHandler(context, tokenGenerator, authenticateQuery, logService).Handle(command, cancellationToken);
            }
            catch (CustomException ex)
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error,
                    string.Concat(GlobalConstantMessages.LOGERRORREFRESHTOKENCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
                throw;
            }
        }
    }
}
