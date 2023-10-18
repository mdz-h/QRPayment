#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-06.
// Comment: Command Assigment Permission to Role.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.AssigmentPermissionstorole
{
    public class AssigmentPermissionToRoleCommandValidator : AbstractValidator<AssigmentPermissionToRoleCommand>
    {
        private readonly IApplicationDbContext context;

        public AssigmentPermissionToRoleCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(v => v.WorkgroupId)
                .NotNull().WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTNULL).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateWorkgroupId).WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(v => v.PermissionId)
                .NotEmpty().WithMessage(GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotNull().WithMessage(GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTNULL).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .Must(ValidateGreaterThanOrEqualTo).WithMessage(GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTZERO).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidatePermissionId).WithMessage(GlobalConstantMessages.ASSIGMENTPERMISSIONIDNOTEXIST).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(v => v).MustAsync(ValidateRegisterInDB).WithMessage(GlobalConstantMessages.PERMISSIONEXISTRECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// method that validates if the value of workgroup exists and if it is active
        /// </summary>
        /// <param name="WorkgroupId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>bool value true or false</returns>
        private async Task<bool> ValidateWorkgroupId(int WorkgroupId, CancellationToken cancellationToken)
        {
            return await this.context.WORKGROUP.AsNoTracking()
                .AnyWithNoLockAsync(a => a.WORKGROUP_ID == WorkgroupId && a.IS_ACTIVE, cancellationToken);
        }

        /// <summary>
        /// </summary>
        /// <param name="PermissionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private bool ValidateGreaterThanOrEqualTo(List<int> PermissionId)
        {
            return !PermissionId.Any(x => x <= 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PermissionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ValidatePermissionId(List<int> PermissionId, CancellationToken cancellationToken)
        {
            var result = await context.PERMISSION.AsNoTracking().Where(w => PermissionId.Contains(w.PERMISSION_ID)).CountAsync(cancellationToken);
            return result == PermissionId.Count;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ValidateRegisterInDB(AssigmentPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await this.context.WORKGROUP_PERMISSION_LINK.AsNoTracking()
                    .Where(w => request.PermissionId.Contains(w.PERMISSION_ID)
                            && w.WORKGROUP_ID == request.WorkgroupId && w.IS_ACTIVE).CountAsync(cancellationToken);
            return result == 0;
        }


    }
}
