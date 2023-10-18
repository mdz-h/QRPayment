#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-12-06.
// Comment: Validator update external apps.
//===============================================================================
#endregion
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.UpdateExternalApps
{
    public class UpdateExternalAppsCommandValidator : AbstractValidator<UpdateExternalAppsCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="context">"Inject" the application context instance</param>        
        public UpdateExternalAppsCommandValidator(IApplicationDbContext context, IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            this.context = context;                    
           
            RuleFor(x => x.ExternalAppId).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Name).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(GlobalConstantMessages.EXTERNALAPPSLENGTHNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.IsActive).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYISACTIVE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x)
               .MustAsync(ValidateExternalApplicationExists).WithMessage(GlobalConstantMessages.EXTERNALAPPNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
               .MustAsync(ValidateExternalApplicationInactive).WithMessage(x => string.Format(GlobalConstantMessages.EXTEWRNALAPPISFALSE, x.ExternalAppId)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Contains the validation for check if already exists a external app item.        
        /// </summary>
        /// <param name="code">Contains id value for a external app.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateExternalApplicationExists(UpdateExternalAppsCommand externalApp, CancellationToken cancellationToken)
        {
            return isValid = await context.EXTERNAL_APPLICATION.AsNoTracking().AnyWithNoLockAsync(x => x.EXTERNAL_APPLICATION_ID.ToString() == externalApp.ExternalAppId, cancellationToken);
        }

        /// <summary>
        /// Contains the validation for check if already inactive a external application item.        
        /// </summary>
        /// <param name="code">Contains id value for a external application.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Logic value to indicate if the item exists</returns>
        private async Task<bool> ValidateExternalApplicationInactive(UpdateExternalAppsCommand externalApp, CancellationToken cancellationToken)
        { 
            if (isValid)
            {
                isValid = (externalApp.IsActive ?? false) || await context.EXTERNAL_APPLICATION.AsNoTracking().AnyWithNoLockAsync(x => x.EXTERNAL_APPLICATION_ID.ToString() == externalApp.ExternalAppId && x.IS_ACTIVE, cancellationToken);
            }

            return isValid;
        } 
    }
}
