#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-24.
// Comment: Validator Roles.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.CreateRoles
{
    public class CreateRolesCommandValidator : AbstractValidator<CreateRolesCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public CreateRolesCommandValidator(IApplicationDbContext context, IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            this.context = context;
            RuleFor(x => x.Code).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYCODE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateRoleExists).WithMessage(GlobalConstantMessages.CODEALREADY).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            RuleFor(x => x.ShortName).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYSHORTNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Description).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// Contains the validation for duplicate workgroup items.        
        /// </summary>
        /// <param name="code">Contains code value for a workgroup.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>        
        private async Task<bool> ValidateRoleExists(string code, CancellationToken cancellationToken)
        {
            return !await context.WORKGROUP.AsNoTracking().AnyWithNoLockAsync(x => x.CODE == code, cancellationToken);
        }
    }
}
