using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.CreateAdministrators
{
    /// <summary>
    /// Class DTO to transfer information to create administrators
    /// </summary>
    public class CreateAdministratorCommand : BasePropertiesDto, IRequest<bool>
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastNamePat { get; set; } = string.Empty;
        public string LastNameMat { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Create Process
    /// </summary>
    public class CreateAdministratorHandler : IRequestHandler<CreateAdministratorCommand, bool>
    {
        private readonly ILogService logService;
        private readonly IApplicationDbContext context;
        private readonly ICryptographyService cryptographyService;
        private readonly ISecurity security;
        private readonly IEmail email;


        /// <summary>
        /// Constructor that injects the interface ILogService and ICreateAdministrator.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="context">Inject the interface necessary for create administrators</param>
        public CreateAdministratorHandler(ILogService logService, IApplicationDbContext context, ICryptographyService cryptographyService, ISecurity security, IEmail email)
        {
            this.logService = logService;
            this.context = context;
            this.cryptographyService = cryptographyService;
            this.security = security;
            this.email = email;
        }

        /// <summary>
        /// Create Administrator
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List DeviceResponse</returns>
        public async Task<bool> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
        {
            bool result = true;

            try
            {
                List<SystemParam> lstSystemParams = await security.GetSystemParamsPasswordRules(cancellationToken);
                int iDays = lstSystemParams.Where(w => w.PARAM_CODE == GlobalConstantHelpers.PASSWORDEXPIRATIONTIME).Select(s => Convert.ToInt32(s.PARAM_VALUE)).FirstOrDefault();

                DateTime dtFecha = DateTime.UtcNow;
                string password = GetRandomPassword(lstSystemParams);
                string hashPassword = cryptographyService.EncryptHash(password);
                Person person = new()
                {
                    NAME = request.Name,
                    MIDDLE_NAME = request.MiddleName,
                    LASTNAME_PAT = request.LastNamePat,
                    LASTNAME_MAT = request.LastNameMat,
                    EMAIL = request.Email,
                    IS_ACTIVE = true,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = dtFecha,
                    OPERATOR = new()
                    {
                        USER_NAME = request.UserName,
                        FL_INTRN = true,
                        IS_ACTIVE = true,
                        OPERATOR_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                        CREATED_DATETIME = dtFecha,
                        OPERATOR_PASSWORD = new List<OperatorPassword>()
                        {
                            new OperatorPassword() {
                                PASSWORD =  hashPassword,
                                EXPIRATION_TIME = dtFecha.AddDays(iDays),
                                IS_ACTIVE = true,
                                CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                                CREATED_DATETIME = dtFecha
                            }
                        }
                    }
                };

                await context.PERSON.AddAsync(person, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPOSTADMINISTRATORS, GlobalConstantHelpers.METHODCREATEADMINISTRATOSHANDLER, LogTypeEnum.Information, $"{GlobalConstantMessages.LOGSENDEMAILCREATEADMINISTRATORCOMMAND} {person.NAME} {person.LASTNAME_PAT} {person.LASTNAME_MAT} {person.EMAIL}", GlobalConstantHelpers.NAMECLASSCREATEADMINISTRATOSCOMMAND);
                SendEmail(person, password, (request.Token is null) ? string.Empty : request.Token);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPOSTADMINISTRATORS, GlobalConstantHelpers.METHODCREATEADMINISTRATOSHANDLER, LogTypeEnum.Error, string.Concat(GlobalConstantMessages.LOGERRORCREATEADMINISTRATORCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSCREATEADMINISTRATOSCOMMAND);
                throw;
            }

            return result;
        }

        /// <summary>
        /// This method generated a random password
        /// </summary>
        /// <param name="config">List System Parameters</param>
        /// <returns>random password</returns>
        public string GetRandomPassword(List<SystemParam> config)
        {
            string password = security.BuildPassword(config);
            bool isValid = security.ValidatePassword(config, password, false);

            if (!isValid)
            {
                return GetRandomPassword(config);
            }
            else
            {
                return password.ToString();
            }
        }

        /// <summary>
        /// This function is responsible that send email
        /// </summary>
        /// <param name="person"></param>        
        /// <param name="password"></param>    
        /// <param name="token"></param>    
        /// <returns>Bolean</returns>
        private async Task SendEmail(Person person, string password, string token)
        {
            List<string> ListOfEmailsAcounts = new()
            {
                (person.EMAIL is null) ? string.Empty : person.EMAIL
            };

            EmailDto emailDto = new()
            {
                TemplateId = GlobalConstantHelpers.IDOFTEMPLATEEMAILNEWADMINISTRATOR,
                Parameters = new SendEmailParametersDto
                {
                    Name = $"{person.NAME} {person.LASTNAME_PAT} {person.LASTNAME_MAT}",
                    Password = password,
                    Date = DateTime.UtcNow,
                },
                ReceiverEmails = ListOfEmailsAcounts.ToArray()
            };
            await email.SendEmail(emailDto, token);
        }
    }
}
