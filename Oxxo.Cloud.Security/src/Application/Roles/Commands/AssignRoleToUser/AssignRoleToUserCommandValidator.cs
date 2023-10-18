#region File Information
//===============================================================================
// Author:  Francisco Ivan Ramirez Alcaraz (NEORIS).
// Date:    2022-12-08.
// Comment: Assign Role To User.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        private readonly IApplicationDbContext context;

        public AssignRoleToUserCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(V => V.Guid)
                .NotEmpty().WithMessage(GlobalConstantMessages.ASSIGMENTROLETOUSERWORKGORUPGUID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateGuid).WithMessage(GlobalConstantMessages.ASSIGMENTGUIDNOTEXIST).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(v => v.WorkgroupId).GreaterThan(0).WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTZERO).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateWorkgroupId).WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDNOTEXIST).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(v => v).MustAsync(ValidateRegisterInDB).WithMessage(GlobalConstantMessages.ASSIGMENTWORKGROUPIDANDGUIDEXIST).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }


        /// <summary>
        /// method that validates if the value of Guid exists and if it is active in OPERATOR
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>bool value true or false</returns>
        private async Task<bool> ValidateGuid(string? Guid, CancellationToken cancellationToken)
        {
            return await this.context.OPERATOR.AsNoTracking()
                .AnyWithNoLockAsync(a => a.OPERATOR_ID.ToString() == Guid && a.IS_ACTIVE, cancellationToken);
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
        /// Method that validate if the register exist in DB
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ValidateRegisterInDB(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var result = await this.context.USER_WORKGROUP_LINK.AsNoTracking()
                    .AnyWithNoLockAsync(a => a.GUID.ToString() == request.Guid && a.WORKGROUP_ID == request.WorkgroupId);
            return !result;
        }
    }
}
