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
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.UpdatePermissions
{
    /// <summary>
    /// Class to validate update permissions
    /// </summary>
    public class UpdatePermissionsCommandValidator : AbstractValidator<UpdatePermissionsCommand>
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
        public UpdatePermissionsCommandValidator(IApplicationDbContext context, IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            this.context = context;
            RuleFor(x => x.PermissionID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotEqual(0).WithMessage(GlobalConstantMessages.PERMISSIONID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

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

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONISACTIVENULL).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
                .MustAsync(ValidatePermission).WithMessage(GlobalConstantMessages.PERMISSIONOTRECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidatePermissionExists).WithMessage(obj => string.Format(GlobalConstantMessages.PERMISSIONEXISTS, obj.Name)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidatePermissionType).WithMessage(obj => string.Format(GlobalConstantMessages.PERMISSIONTYPEEXISTS, obj.PermissionTypeID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidateModule).WithMessage(obj => string.Format(GlobalConstantMessages.MODULEEXISTS, obj.ModuleID)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        } 
        
        /// <summary>
        /// Validate if exists module
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidateModule(UpdatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await this.context.MODULE.AsNoTracking().Where(w => w.MODULE_ID == permission.ModuleID).AnyAsync(cancellationToken);
            }
            return isValid;
        }

        /// <summary>
        /// Validate if exists permission type
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidatePermissionType(UpdatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await this.context.PERMISSION_TYPE.AsNoTracking().Where(w => w.PERMISSION_TYPE_ID == permission.PermissionTypeID).AnyAsync(cancellationToken);
            }

            return isValid;
        }

        /// <summary>
        /// Validate if exists permission
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidatePermissionExists(UpdatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                bool lExiste = await this.context.PERMISSION.AsNoTracking().Where(w => w.MODULE_ID == permission.ModuleID && (w.NAME ?? "").ToLower() == (permission.Name ?? "").ToLower() && w.PERMISSION_ID != permission.PermissionID).AsNoTracking().AnyAsync(cancellationToken);
                isValid = !lExiste;
            }
            return isValid;
        }

        /// <summary>
        /// Validate if exists permission
        /// </summary>
        /// <param name="permission">Permission class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidatePermission(UpdatePermissionsCommand permission, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.PERMISSION.AsNoTracking().Where(w => w.PERMISSION_ID == permission.PermissionID).AnyAsync(cancellationToken);
            }

            return isValid;
        }
    }
}
