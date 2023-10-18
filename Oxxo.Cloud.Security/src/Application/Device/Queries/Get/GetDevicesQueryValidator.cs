#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-11-26.
// Comment: Get devices query validator.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Device.Queries.Get
{
    public class GetDevicesQueryValidator : AbstractValidator<GetDevicesQuery>
    {
        private readonly IDeviceQueryGet deviceQuery;
        private bool validate = false;

        /// <summary>
        /// Constructor GetDevicesQueryValidator that injects the interface IDeviceQuery and IvalidateTokenQueryHelper . 
        /// </summary>        
        /// <param name="deviceQuery"></param>
        /// <param name="validateTokenQueryHelper"></param>
        public GetDevicesQueryValidator(IDeviceQueryGet deviceQuery)
        {
            this.deviceQuery = deviceQuery;
            RuleFor(x => x.PageNumber)
              .NotNull().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ItemsNumber)
              .NotNull().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            
            RuleFor(request => request)
            .Must(ValidateParamsTypes).WithMessage(GlobalConstantMessages.LOGVALIDATEPARAMSGETDEVICES).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
            .MustAsync(ValidateIfExistsDevices).WithMessage(GlobalConstantMessages.LOGNOTFOUNDGETDEVICES).WithErrorCode(((int)HttpStatusCode.NoContent).ToString());
        }

        /// <summary>
        /// This function is responsible that validate params type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>   
        /// <returns>bool</returns>
        private bool ValidateParamsTypes(GetDevicesQuery request)
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
        /// This function is responsible that validate if exists devices
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>   
        /// <returns>bool</returns>
        private async Task<bool> ValidateIfExistsDevices(GetDevicesQuery request, CancellationToken cancellationToken)
        {
            if (validate)
            {
                return await deviceQuery.ValidateIfExistsDevices(request);
            }
            return true;
        }

    }
}
