#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-29.
// Comment: Class Roles By User ID validator.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByUserIdQueryValidator : AbstractValidator<RolesByUserIdQuery>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public RolesByUserIdQueryValidator(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(x => x.UserId).NotEmpty().WithMessage(GlobalConstantMessages.USERWORKGROUPLINKEMPTYGUID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateUserWorkgroupLinkExists).WithMessage(GlobalConstantMessages.USERWORKGROUPLINKNOTFOUND).WithErrorCode(((int)HttpStatusCode.NoContent).ToString());    
        }

        /// <summary>
        /// Contains the validation for get workgroup item to delete.        
        /// </summary>
        /// <param name="code">Contains code value for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateUserWorkgroupLinkExists(string userId, CancellationToken arg2)
        {
            return await context.USER_WORKGROUP_LINK.AsNoTracking().AnyAsync(x => x.GUID.ToString() == userId && x.IS_ACTIVE);
        }
    }
}
