#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-21.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.GenerateApiKey
{
    /// <summary>
    /// This class validate the parameters value to generate API Key
    /// </summary>
    public class GenerateApiKeyCommandValidator : AbstractValidator<GenerateApiKeyCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor class
        /// </summary>
        public GenerateApiKeyCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(x => x.ExternalAppId)
                .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateExternalApp).WithMessage(GlobalConstantMessages.NOTRECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Validate if the external id exists in the database
        /// </summary>
        /// <param name="externalAppId">external APP id</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>logic value</returns>
        private async Task<bool> ValidateExternalApp(string externalAppId, CancellationToken cancellationToken)
        {
            return await context.EXTERNAL_APPLICATION.Where(w => w.EXTERNAL_APPLICATION_ID.ToString() == externalAppId && w.IS_ACTIVE).AnyAsync(cancellationToken);
        }
    }
}
