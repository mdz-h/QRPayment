#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrator.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.DeleteAdministrators
{
    /// <summary>
    /// Principal Class Administrator to validate
    /// </summary>
    public class DeleteAdministratorsCommandValidator : AbstractValidator<DeleteAdministratorsCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="authenticateQuery">Auth</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public DeleteAdministratorsCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(x => x.UserId)
               .NotEmpty().WithMessage(GlobalConstantMessages.USERIDINVALID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
                .MustAsync(ValidateAdministrators).WithMessage(GlobalConstantMessages.NORECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Validate if exists Administrators
        /// </summary>
        /// <param name="administrators">Administrators class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is succesfull</returns>
        private async Task<bool> ValidateAdministrators(DeleteAdministratorsCommand administrators, CancellationToken cancellationToken)
        {
            return await context.OPERATOR.AsNoTracking().AnyWithNoLockAsync(w => w.OPERATOR_ID.ToString() == (administrators.UserId ?? string.Empty).ToString(), cancellationToken);
        }
    }
}
