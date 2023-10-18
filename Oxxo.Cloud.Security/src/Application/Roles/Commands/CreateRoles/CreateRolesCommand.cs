#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-23.
// Comment: Command workgroup.
//===============================================================================
#endregion
using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.CreateRoles
{
    public class CreateRolesCommand : BasePropertiesDto, IRequest<bool>
    {
        public string ShortName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateWorkgroupHandler : IRequestHandler<CreateRolesCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>
        /// <param name="logService">"Inject" the log instance</param>       
        public CreateWorkgroupHandler(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }

        /// <summary>
        /// Contains all the business rules necessary to create the workgroup item.        
        /// </summary>
        /// <param name="request">Contains the main values for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate the operation result</returns>
        /// <exception cref="CustomException">Contains the internal generated error</exception>
        public async Task<bool> Handle(CreateRolesCommand request, CancellationToken cancellationToken)
        {
            bool lResult;
            try
            {
                var workgroup = new Workgroup()
                {
                    NAME = request.ShortName,
                    CODE = request.Code,
                    DESCRIPTION = request.Description,
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                    CREATED_DATETIME = DateTime.UtcNow
                };

                await this.context.WORKGROUP.AddAsync(workgroup, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);                
                lResult = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODCREATEWORKGROUP, GlobalConstantHelpers.METHODCREATEWORKGROUP, LogTypeEnum.Error, request.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORCREATEWORKGROUPCOMMAND, ex.InnerException == null ? ex.Message : ex.InnerException.Message, request.Code), GlobalConstantHelpers.NAMECLASSCREATEROLECOMMANDHANDLER);
                throw new CustomException(ex.InnerException == null ? ex.Message : ex.InnerException.Message, HttpStatusCode.InternalServerError);
            }
            return lResult;
        }
    }

}
