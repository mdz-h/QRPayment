#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrator.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.DeleteAdministrators
{
    /// <summary>
    /// Principal Class Administrator command
    /// </summary>
    public class DeleteAdministratorsCommand : BasePropertiesDto, IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Principal Class
    /// </summary>
    public class DeleteAdministratorsHandler : IRequestHandler<DeleteAdministratorsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the CreatePermissionsHandler.
        /// </summary>
        /// <param name="ILogService">"Inject" the log instance</param>
        /// <param name="context">Inject context</param>
        public DeleteAdministratorsHandler(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context;
        }

        /// <summary>
        /// Principal method
        /// </summary>
        /// <param name="request">Request App</param>
        /// <param name="cancellationToken">CancelationToken</param>
        /// <returns>logic value to indicated the result to operation</returns>
        public async Task<bool> Handle(DeleteAdministratorsCommand request, CancellationToken cancellationToken)
        {
            bool result;
            try
            {               
                var Operator = await context.OPERATOR.Where(w => w.OPERATOR_ID.ToString() == request.UserId).FirstAsync(cancellationToken);
                Operator.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                Operator.MODIFIED_DATETIME = DateTime.UtcNow;
                Operator.IS_ACTIVE = false;
                Operator.LCOUNT++;
                await context.SaveChangesAsync(cancellationToken);                
                result = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS, GlobalConstantHelpers.METHODADMINISTRATORSDELETE, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIDELETE, ex.GetException()), GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }

            return result;
        }
    }

}
