#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-13.
// Comment: Class Query of ExternalApps Validator.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using System.Net;
using ConstLog = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantMessages;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get
{
    public class GetExternalAppsQueryValidator : AbstractValidator<GetExternalAppsQuery>
    {
        #region Properties
        /// <summary>
        /// Contract of ExternalAppsQueryGet
        /// </summary>
        private readonly IExternalAppsQueryGet externalAppsQueryGet;

        /// <summary>
        /// Field of Validate
        /// </summary>
        private bool validate = false;
        #endregion
        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="externalAppsQueryGet"></param>
        public GetExternalAppsQueryValidator(IExternalAppsQueryGet externalAppsQueryGet)
        {
            this.externalAppsQueryGet = externalAppsQueryGet;
            RuleFor(request => request)
            .Must(ValidateParamsTypes).WithMessage(ConstLog.EXTERNALAPPS_INVALIDAPARAMETERS).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .MustAsync(ValidateIfExists).WithMessage(ConstLog.EXTERNALAPPS_NOTFOUND).WithErrorCode(((int)HttpStatusCode.NoContent).ToString());
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Validate Params Types
        /// </summary>
        /// <param name="request">Data to validate</param>
        private bool ValidateParamsTypes(GetExternalAppsQuery request)
        {
            if (request == null)
            {
                return validate = false;
            }
            if (request.PageNumber == 0 || request.PageNumber < 0)
            {
                return validate = false;
            }
            if (request.ItemsNumber == 0 || request.ItemsNumber < 0)
            {
                return validate = false;
            }
            return validate = true;
        }

        /// <summary>
        /// Validate if exists data
        /// </summary>
        /// <param name="request">Data to filter</param>
        private async Task<bool> ValidateIfExists(GetExternalAppsQuery request, CancellationToken cancellationToken)
        {
            if (validate)
            {
                return await externalAppsQueryGet.ValidateIfExists(request);
            }
            return true;
        }
        #endregion
    }
}