#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-28.
// Comment: Validator update Roles.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.UpdateRoles
{
    public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
    {
        private readonly IApplicationDbContext context;
        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public UpdateRolesCommandValidator(IApplicationDbContext context) {
            this.context = context;  
           
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());            
            RuleFor(x => x.ShortName).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYSHORTNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Description).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.IsActive).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYISACTIVE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x)
               .MustAsync(ValidateRoleExists).WithMessage(GlobalConstantMessages.WORKGROUPNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Contains the validation for duplicate workgroup items.        
        /// </summary>
        /// <param name="code">Contains code value for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateRoleExists(UpdateRolesCommand role, CancellationToken cancellationToken)
        {
            return await context.WORKGROUP.AsNoTracking().AnyWithNoLockAsync(x => x.WORKGROUP_ID == role.RoleId, cancellationToken);
        }
    }
}
