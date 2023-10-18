#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    30/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Queries
{
    /// <summary>
    /// Class Validator
    /// </summary>
    public class PermissionsQueryByIdValidator : AbstractValidator<PermissionsQueryById>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryByIdValidator(IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            RuleFor(x => x.permissionID)
               .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }
    }
}
