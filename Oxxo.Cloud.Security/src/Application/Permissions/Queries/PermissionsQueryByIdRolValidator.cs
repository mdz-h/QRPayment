#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    30/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Queries
{
    /// <summary>
    /// Clas Validator
    /// </summary>
    public class PermissionsQueryByIdRolValidator : AbstractValidator<PermissionsQueryByIdRol>
    {
        /// <summary>
        /// Constructor Class
        /// </summary>        
        public PermissionsQueryByIdRolValidator()
        {
            RuleFor(x => x.roleId)
               .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONROLEID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .NotEqual(0).WithMessage(GlobalConstantMessages.PERMISSIONROLEID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }        
    }
}
