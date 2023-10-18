#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-13.
// Comment: Class Query of ExternalApps.
//===============================================================================
#endregion

using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using System.Net;
using Const = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantHelpers;
using ConstLog = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantMessages;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get
{
    public class GetExternalAppsQuery : BasePropertiesDto, IRequest<List<ExternalAppsResponse>>
    {
        public GetExternalAppsQuery()
        {
            Identifier = string.Empty;
        }

        public string Identifier { get; set; }
        public int ItemsNumber { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetExternalAppsHandler : IRequestHandler<GetExternalAppsQuery, List<ExternalAppsResponse>>
    {
        #region Properties
        /// <summary>
        /// Contract of LogService
        /// </summary>
        private readonly ILogService _logService;

        /// <summary>
        /// Contract of ExternalAppsQueryGet
        /// </summary>
        private readonly IExternalAppsQueryGet _externalAppsQueryGet;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logService">Contract of LogService</param>
        /// <param name="externalAppsQueryGet">Contract of ExternalAppsQueryGet</param>
        public GetExternalAppsHandler(ILogService logService, IExternalAppsQueryGet externalAppsQueryGet)
        {
            _logService = logService;
            _externalAppsQueryGet = externalAppsQueryGet;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Generates the DeviceResponse 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List DeviceResponse</returns>
        public async Task<List<ExternalAppsResponse>> Handle(GetExternalAppsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                List<ExternalAppsResponse> listResponse = await _externalAppsQueryGet.Get(request);
                if (!listResponse.Any())
                {
                    throw new CustomException(ConstLog.EXTERNALAPPS_EMPTY, HttpStatusCode.NoContent);
                }              
                return listResponse ?? new List<ExternalAppsResponse>();
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.EVENTEXTERNALAPPSGET,
                    Const.EXTERNALAPPSMETHODGETHANDLER,
                    LogTypeEnum.Error,
                    request.UserIdentification,
                    string.Concat(ConstLog.EXTERNALAPPS_ERROR_GET_QUERY, ex.GetException()),
                    Const.NAMECLASSGETEXTERNALAPPSQUERY);
                throw;
            }
        }
        #endregion
    }
}