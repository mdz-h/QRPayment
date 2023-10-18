#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-28.
// Comment: Validator delete Roles.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteRolesCommandValidator : AbstractValidator<DeleteRolesCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public DeleteRolesCommandValidator(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x)
                .MustAsync(ValidateRoleExists).WithMessage(GlobalConstantMessages.ROLENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());           
            
        }

        /// <summary>
        /// Contains the validation for get workgroup item to delete.        
        /// </summary>
        /// <param name="code">Contains code value for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateRoleExists(DeleteRolesCommand role, CancellationToken arg2)
        {
            return await context.WORKGROUP.AsNoTracking().AnyAsync(x => x.WORKGROUP_ID == role.RoleId && x.IS_ACTIVE, arg2);
        }
    }
}
