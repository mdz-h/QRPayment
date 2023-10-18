#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-12-08.
// Comment: Validator delete external apps.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteExternalApps
{
    public class DeleteExternalAppsCommandValidator : AbstractValidator<DeleteExternalAppsCommand>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public DeleteExternalAppsCommandValidator(IApplicationDbContext context, IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            this.context = context;
            RuleFor(x => x.ExternalAppId).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x)
                .MustAsync(ValidateExternalApplicationExists).WithMessage(GlobalConstantMessages.EXTERNALAPPNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Contains the validation for check if exists a exernal app item.        
        /// </summary>
        /// <param name="externalApp">id value for a external app item.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateExternalApplicationExists(DeleteExternalAppsCommand externalApp, CancellationToken cancellationToken)
        {
            return await context.EXTERNAL_APPLICATION.AsNoTracking().AnyAsync(x => x.EXTERNAL_APPLICATION_ID.ToString() == externalApp.ExternalAppId && x.IS_ACTIVE, cancellationToken);
        }
    }
}