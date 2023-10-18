#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-08.
// Comment: Class Validator of External Apps.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.CreateExternalApps
{
    public class CreateExternalAppsCommandValidators : AbstractValidator<CreateExternalAppsCommand>
    {
        #region Properties
        /// <summary>
        /// Contract of ApplicationDbContext
        /// </summary>
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Field of Validate
        /// </summary>
        private bool _validate = false;
        #endregion
        #region Contructor
        /// <summary>
        /// Contsructor
        /// </summary>
        /// <param name="context">Contract of AplicationDbContext</param>
        public CreateExternalAppsCommandValidators(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(v => v.Name).NotEmpty().WithMessage(GlobalConstantMessages.DEVICENAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Code).NotEmpty().WithMessage(GlobalConstantMessages.CODEEXTERNALNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateCode).WithMessage(GlobalConstantMessages.CODEEXTERNALNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Validate Code
        /// </summary>
        /// <param name="code">Code of ExternalAplication</param>
        /// <param name="cancellationToken">CancellationToken</param>
        private async Task<bool> ValidateCode(string? code, CancellationToken cancellationToken)
        {
            if (!_validate || string.IsNullOrEmpty(code))
            {
                return true;
            }
            return _validate = await context.EXTERNAL_APPLICATION.AsNoTracking().AnyAsync(x => x.CODE == code && x.IS_ACTIVE, cancellationToken: cancellationToken);
        }
        #endregion
    }
}