#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-06.
// Comment: Class of Create External Apps.
//===============================================================================
#endregion

using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.CreateExternalApps
{
    public class CreateExternalAppsCommand : BasePropertiesDto, IRequest<bool>
    {
        public CreateExternalAppsCommand()
        {
            Code = string.Empty;
            Name = string.Empty;
        }

        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CreateExternalAppsCommandHandle : IRequestHandler<CreateExternalAppsCommand, bool>
    {
        #region Properties
        /// <summary>
        /// Contract of ApplicationDbContext
        /// </summary>
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Contract of LogService
        /// </summary>
        private readonly ILogService logService;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contract of ApplicationDbContext</param>
        /// <param name="logService">Contract of logService</param>
        public CreateExternalAppsCommandHandle(IApplicationDbContext context, ILogService logService)
        {
            this.context = context;
            this.logService = logService;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request">Object request</param>
        /// <param name="cancellationToken">CancellationToke</param>
        public async Task<bool> Handle(CreateExternalAppsCommand request, CancellationToken cancellationToken)
        {
            bool Out = false;
            try
            {
                var row = await context.EXTERNAL_APPLICATION.Where(x => x.CODE == request.Code && x.IS_ACTIVE).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (row == null)
                {
                    ExternalApplication registry = new()
                    {
                        CODE = request.Code,
                        NAME = request.Name,
                        TIME_EXPIRATION_TOKEN = GlobalConstantHelpers.TIMEOUTEXPIRATIONTOKENMINUTS,
                        IS_ACTIVE = true,
                        LCOUNT = 0,
                        CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                        CREATED_DATETIME = DateTime.UtcNow
                    };
                    await context.EXTERNAL_APPLICATION.AddAsync(registry, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    Out = true;
                }
                else
                {
                    await logService.Logger(GlobalConstantHelpers.EVENTCREATEEXTERNALAPPLICATION
                        , GlobalConstantHelpers.METHODEXTERNALAPPCRAETEHANDLER
                        , LogTypeEnum.Error
                        , request.UserIdentification
                        , string.Concat(GlobalConstantMessages.LOG_ERROR_EXTERNALAPPCREATECOMMANDEMPTY, request.Code)
                        , GlobalConstantHelpers.NAMECLASSEXTERNALAPPCOMMANDCREATE);

                    throw new CustomException(string.Concat(GlobalConstantMessages.LOG_ERROR_EXTERNALAPPCREATECOMMANDEMPTY, request.Code), HttpStatusCode.UnprocessableEntity);
                }
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTCREATEEXTERNALAPPLICATION
                     , GlobalConstantHelpers.METHODEXTERNALAPPCRAETEHANDLER
                     , LogTypeEnum.Error
                     , request.UserIdentification
                     , string.Concat(GlobalConstantMessages.LOG_ERROR_EXTERNALAPPCREATECOMMAND, ex.GetException())
                     , GlobalConstantHelpers.NAMECLASSEXTERNALAPPCOMMANDCREATE);
                throw;
            }
            return Out;
        }
        #endregion
    }
}