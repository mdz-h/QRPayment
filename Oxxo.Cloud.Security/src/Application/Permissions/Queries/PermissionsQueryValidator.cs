#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    29/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Queries
{
    /// <summary>
    /// Class Validator
    /// </summary>
    public class PermissionsQueryValidator : AbstractValidator<PermissionsQuery>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
               .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ItemsNumber)
              .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }
    }
}
