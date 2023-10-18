#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    2023-01-03.
// Comment: Class of Create relate External application with a role.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.AssignExternalAppsToRole
{
    /// <summary>
    /// This class validate the rules of request parameter to create a relate external applications with roles
    /// </summary>
    public class AssignExternalAppsToRoleCommandValidator : AbstractValidator<AssignExternalAppsToRoleCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid = true;
             

        /// <summary>
        /// Initial class to configuration parameters 
        /// </summary>
        /// <param name="context">database context</param>
        public AssignExternalAppsToRoleCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(v => v.WorkgroupId)
                .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.ExternalAppId)
               .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
               .MustAsync(ValidateExternalApp).WithMessage(GlobalConstantMessages.NOTFOUNDEXTERNALAPP).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
               .MustAsync(ValidateRole).WithMessage(GlobalConstantMessages.NOTFOUNDROLE).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
               .MustAsync(ValidateRelationship).WithMessage(x => string.Format(GlobalConstantMessages.EXTERNALAPPWASRELATIONSHIP, x.WorkgroupId, x.ExternalAppId)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Validate if the role id is relationship with a external application
        /// </summary>
        /// <param name="roleCommand">Request parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>logic value</returns>
        public async Task<bool> ValidateRelationship(AssignExternalAppsToRoleCommand roleCommand, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.USER_WORKGROUP_LINK.AsNoTracking()
                                .AnyWithNoLockAsync(w => w.WORKGROUP_ID == roleCommand.WorkgroupId
                                        && w.GUID.ToString() == roleCommand.ExternalAppId
                                        && w.IS_ACTIVE, cancellationToken);
            }

            return !isValid; 
        }


        /// <summary>
        /// Validate if the work group  exits in the database
        /// </summary>
        /// <param name="roleCommand">Request parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>logic value</returns>
        public async Task<bool> ValidateRole(AssignExternalAppsToRoleCommand roleCommand, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.WORKGROUP.AsNoTracking().AnyWithNoLockAsync(w => w.WORKGROUP_ID == roleCommand.WorkgroupId && w.IS_ACTIVE, cancellationToken);
            }

            return isValid;
        }

        /// <summary>
        /// Validate if the external application exits in the database
        /// </summary>
        /// <param name="roleCommand">Request parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>logic value</returns>
        public async Task<bool> ValidateExternalApp(AssignExternalAppsToRoleCommand roleCommand, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.EXTERNAL_APPLICATION.AsNoTracking().AnyWithNoLockAsync(w => w.EXTERNAL_APPLICATION_ID.ToString() == roleCommand.ExternalAppId && w.IS_ACTIVE, cancellationToken);
            }

            return isValid;
        }
    }
}
