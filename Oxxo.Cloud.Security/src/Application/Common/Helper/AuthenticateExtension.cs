using Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate;
using Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginUser;
using Oxxo.Cloud.Security.Application.Auth.Commands.Login;
using Oxxo.Cloud.Security.Application.Auth.Commands.LoginExternal;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Helper
{
    public static class AuthenticateExtension
    { 
        public static IAuthenticate GetAuthenticateType(this AuthenticateCommand authenticate,  IApplicationDbContext context, ITokenGenerator tokenGenerator, IAuthenticateQuery authenticateQuery, ILogService logService, ICryptographyService cryptographyService, ISecurity security)
        {
            IAuthenticate authClass;

            switch (authenticate.TypeAuth)
            {
                case GlobalConstantHelpers.DEVICE:
                    authClass = new AuthenticateDevice(context, tokenGenerator, authenticateQuery, logService, new() { TypeAuth = authenticate.TypeAuth, Id = authenticate.Id, DeviceKey = authenticate.DeviceKey, Identification = authenticate.Identification });
                    break;

                case GlobalConstantHelpers.EXTERNAL:
                    authClass = new AuthenticateExternal(context, tokenGenerator, logService, new() { TypeAuth = authenticate.TypeAuth, Id = authenticate.Id, ApiKey = authenticate.ApiKey, Identification = authenticate.Identification });
                    break;
                case GlobalConstantHelpers.USER:
                case GlobalConstantHelpers.USERXPOS:
                    authClass = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService, 
                        new()
                        {
                            TypeAuth = authenticate.TypeAuth,
                            Identification = authenticate.Identification,
                            User = authenticate.User,
                            Password = authenticate.Password,
                            CrPlace = authenticate.CrPlace,
                            CrStore = authenticate.CrStore,
                            Till = authenticate.Till,
                            AdministrativeDate = authenticate.AdministrativeDate,
                            ProcessDate = authenticate.ProcessDate,
                            Type = authenticate.Type,
                            IsInternal = authenticate.IsInternal,
                            StatusOperator = authenticate.StatusOperator,
                            WorkGroup = authenticate.WorkGroup,
                            FullName = authenticate.FullName
                        }, cryptographyService, security);
                    break;

                default:
                    throw new CustomException(GlobalConstantMessages.TYPEAUTHNOTFOUND, HttpStatusCode.UnprocessableEntity);
            }

            return authClass;
        }
    }
}
