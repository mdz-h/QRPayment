#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    14/12/2022.
// Comment: Query Administrators.
//===============================================================================
#endregion


using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Queries
{
    /// <summary>
    /// Validate Class
    /// </summary>
    public class AdministratorsQueryValidator : AbstractValidator<AdministratorsQuery>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        /// <param name="authenticateQuery">Authenticate</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public AdministratorsQueryValidator(IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            RuleFor(x => x.PageNumber)
              .NotNull().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ItemsNumber)
              .NotNull().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }
    }
}
