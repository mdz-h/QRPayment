#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for change password of Administrators.
//===============================================================================
#endregion

using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    /// <summary>
    /// Class that define params for request.
    /// </summary>            
    /// <param name="UserId"></param>
    /// <param name="OldPassword"></param>
    /// <param name="NewPassword"></param>
    public class ChangePasswordCommand : BasePropertiesDto, IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IChangePassword Password;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the interface ILogService and IChangePassword.
        /// </summary>            
        /// <param name="logService">Inject the interface necessary for save the log</param>
        /// <param name="Password">Inject the interface IChangePassword</param>
        public ChangePasswordHandler(ILogService logService, IChangePassword Password)
        {
            this.logService = logService;
            this.Password = Password;
        }

        /// <summary>
        /// Handle method
        /// </summary>
        /// <param name="request">>Request</param>
        /// <param name="cancellationToken">CancelationToken</param>
        /// <returns>List DeviceResponse</returns>
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await Password.ChangePasswordAdministratorAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS, GlobalConstantHelpers.METHODCHANGEPASSWORDHANDLER, LogTypeEnum.Error
                    , request.UserIdentification, string.Concat(GlobalConstantMessages.LOGERRORCHANGEPASSWORDCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSCHANGEPASSWORDCOMMAND);
                throw;
            }
        }

    }
}
