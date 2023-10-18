#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-29.
// Comment: Validator Roles by Id.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByIdQueryValidator : AbstractValidator<RolesByIdQuery>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public RolesByIdQueryValidator(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPIDNEGATIVE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateWorkgroupExists).WithMessage(GlobalConstantMessages.WORKGROUPNOTFOUND).WithErrorCode(((int)HttpStatusCode.NoContent).ToString());
        }

        private async Task<bool> ValidateWorkgroupExists(int roleId, CancellationToken arg2)
        {
            return await context.WORKGROUP.AsNoTracking().AnyAsync(x => x.WORKGROUP_ID == roleId && x.IS_ACTIVE, arg2);
        }
    }
}
