#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-26.
// Comment: DELETE API KEY.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteApiKey
{
    /// <summary>
    /// This class validate the parameters value to delete an API Key
    /// </summary>
    public class DeleteApiKeyCommandValidator : AbstractValidator<DeleteApiKeyCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid;
        /// <summary>
        /// Constructor class
        /// </summary>
        public DeleteApiKeyCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(x => x.ExternalAppId)
               .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.APIKey)
               .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
                .MustAsync(ValidateExistsApiKey).WithMessage(GlobalConstantMessages.NORECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
                .MustAsync(ValidateDeleted).WithMessage(string.Format(GlobalConstantMessages.INACTIVERECORD, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Validate if the API Key was not inactive
        /// </summary>
        /// <param name="command">Parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>logic value</returns>
        public async Task<bool> ValidateDeleted(DeleteApiKeyCommand command, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                var entity = await context.API_KEY.FirstOrDefaultWithNoLockAsync(w => w.API_KEY == command.APIKey && w.EXTERNAL_APPLICATION_ID.ToString() == command.ExternalAppId && w.IS_ACTIVE, cancellationToken);
                isValid = entity != null;
            }

            return isValid;
        }

        /// <summary>
        /// Validate if the API Key exists
        /// </summary>
        /// <param name="command">Parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>logic value</returns>
        public async Task<bool> ValidateExistsApiKey(DeleteApiKeyCommand command, CancellationToken cancellationToken)
        {
            return isValid = await context.API_KEY.AnyWithNoLockAsync(w => w.API_KEY == command.APIKey && w.EXTERNAL_APPLICATION_ID.ToString() == command.ExternalAppId, cancellationToken: cancellationToken);
        }
    }
}
