#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    29/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.DeletePermissions
{
    /// <summary>
    /// Class to validate delete permissions
    /// </summary>
    public class DeletePermissionsCommandValidator : AbstractValidator<DeletePermissionsCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid = true;

        /// <summary>
        /// Constructor Class to validate
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="authenticateQuery">Authorize</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public DeletePermissionsCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(x => x.PermissionID)
                .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidatePermission).WithMessage(GlobalConstantMessages.PERMISSIONOTRECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidatePermissionInRole).WithMessage(x => string.Format(GlobalConstantMessages.PERMISSIONHAVEROL, x.PermissionID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidatePermissionActive).WithMessage(x => string.Format(GlobalConstantMessages.PERMISSIONISNOTACTIVE, x.PermissionID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());                
        }

        /// <summary>
        /// Validate if permission exists and be active
        /// </summary>
        /// <param name="permission">Permission id</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Retur if the validate is succefull</returns>
        private async Task<bool> ValidatePermissionActive(int permissionID, CancellationToken cancellationToken)
        {
            return isValid = await context.PERMISSION.AsNoTracking().AnyWithNoLockAsync(w => w.PERMISSION_ID == permissionID && w.IS_ACTIVE, cancellationToken);
        }

        /// <summary>
        /// Validate if exists the permission
        /// </summary>
        /// <param name="permission">Permission id</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Retur if the validate is succefull</returns>
        private async Task<bool> ValidatePermission(int permissionID, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.PERMISSION.AsNoTracking().AnyWithNoLockAsync(w => w.PERMISSION_ID == permissionID, cancellationToken);
            }

            return isValid;
        }

        /// <summary>
        /// Validate if exists permission with role
        /// </summary>
        /// <param name="permission">Permission id</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Retur if the validate is succefull</returns>
        private async Task<bool> ValidatePermissionInRole(int permissionID, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await (from item in context.PERMISSION.AsNoTracking().Where(w => w.PERMISSION_ID == permissionID)
                                           let lTienerol = context.WORKGROUP_PERMISSION_LINK.AsNoTracking().Where(w => w.PERMISSION_ID == item.PERMISSION_ID && w.IS_ACTIVE).Any()
                                           where !lTienerol
                                           select item).AnyWithNoLockAsync();
            }

            return isValid;
        }
    }
}
