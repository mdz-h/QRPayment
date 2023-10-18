#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    28/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.CreatePermissions
{
    /// <summary>
    /// Class validate to Creating permissions
    /// </summary>
    public class CreatePermissionsCommandValidator : AbstractValidator<CreatePermissionsCommand>
    {
        private readonly IApplicationDbContext context;

        private bool isValid = true;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="authenticateQuery">Authorize</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public CreatePermissionsCommandValidator(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONCODE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ModuleID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONMODULEID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.IDVALUEINCORRECT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.PermissionTypeID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONTYPE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.IDVALUEINCORRECT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
               .MustAsync(ValidatePermissionType).WithMessage(obj => string.Format(GlobalConstantMessages.PERMISSIONTYPEEXISTS, obj.PermissionTypeID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
               .MustAsync(ValidateModule).WithMessage(obj => string.Format(GlobalConstantMessages.MODULEEXISTS, obj.ModuleID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
               .MustAsync(ValidatePermissionExists).WithMessage(obj => string.Format(GlobalConstantMessages.PERMISSIONEXISTS, obj.Name)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

        }

        /// <summary>
        /// Validate if exists the module
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidateModule(CreatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await this.context.MODULE.AsNoTracking().AnyWithNoLockAsync(w => w.MODULE_ID == permission.ModuleID, cancellationToken);
            }

            return isValid;
        }

        /// <summary>
        /// Validate if exists the permission type
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidatePermissionType(CreatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await this.context.PERMISSION_TYPE.AsNoTracking().AnyWithNoLockAsync(w => w.PERMISSION_TYPE_ID == permission.PermissionTypeID, cancellationToken);
            }

            return isValid;
        }

        /// <summary>
        /// Validate if exists the permission
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidatePermissionExists(CreatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                bool lExiste = await this.context.PERMISSION.AsNoTracking().AnyWithNoLockAsync(w => w.MODULE_ID == permission.ModuleID && (w.NAME ?? "").ToLower() == (permission.Name ?? "").ToLower(), cancellationToken);
                isValid = !lExiste;
            }

            return isValid;
        }
    }
}
