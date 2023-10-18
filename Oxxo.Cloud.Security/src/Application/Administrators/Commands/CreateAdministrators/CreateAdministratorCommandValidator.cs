using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.Net;
using System.Text.RegularExpressions;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.CreateAdministrators
{

    /// <summary>
    /// Principal class to validate fields
    /// </summary>
    public class CreateAdministratorCommandValidator : AbstractValidator<CreateAdministratorCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid = true;

        /// <summary>
        /// Constructor that injects the interface ILogService and IHelpervalidateTokenQuery.
        /// </summary>     
        /// <param name="context">database context</param>
        /// <param name="authenticateQuery">Authorize</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public CreateAdministratorCommandValidator(IApplicationDbContext context, IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {

            this.context = context;


            RuleFor(v => v.LastNamePat)
                .NotEmpty().WithMessage(GlobalConstantMessages.LASTNAMEPATNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotNull().WithMessage(GlobalConstantMessages.LASTNAMEPATNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage(GlobalConstantMessages.EMAILNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotNull().WithMessage(GlobalConstantMessages.EMAILNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .Must(ValidateEmail).WithMessage(x => string.Format(GlobalConstantMessages.EMAILINVALID, x.Email)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());


            RuleFor(x => x.LastNameMat)
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.MiddleName)
             .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage(GlobalConstantMessages.NAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotNull().WithMessage(GlobalConstantMessages.NAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.UserName)
                .NotEmpty().WithMessage(GlobalConstantMessages.USERNAMEMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .NotNull().WithMessage(GlobalConstantMessages.USERNAMEMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MustAsync(ValidateUserName).WithMessage(GlobalConstantMessages.USERNAMEMEEXIST).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
            
            RuleFor(request => request)
                   .MustAsync(ValidatePasswordRules).WithMessage(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDRULES).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }  
        

        /// <summary>
        /// Validated if the email is correct
        /// </summary>
        /// <param name="email">E-mail Address</param>
        /// <returns>true if the format email is correct</returns>
        private bool ValidateEmail(string email)
        {
            if (isValid)
            {
                string expresion = @"\w+([-+.’]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                isValid = Regex.IsMatch(email, expresion) && (Regex.Replace(email, expresion, string.Empty).Length == 0);
            }
            return isValid;
        }

        /// <summary>
        /// This function validate if the user name not exists 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>true or false if the user name is valid</returns>

        private async Task<bool> ValidateUserName(string userName, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = !await context.OPERATOR.AsNoTracking().AnyWithNoLockAsync(o => o.USER_NAME == userName && o.IS_ACTIVE, cancellationToken);
            }
            return isValid;
        }

        /// <summary>
        /// This method validate if the password rules are configured
        /// </summary>
        /// <param name="request">parameters</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>bool</returns> 
        private async Task<bool> ValidatePasswordRules(CreateAdministratorCommand request, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                List<string> lstParams = new List<string>() { GlobalConstantHelpers.PASSWORDRULES, GlobalConstantHelpers.PASSWORDMINLENGTHRULES, GlobalConstantHelpers.PASSWORDMAXLENGTHRULES };
                var lstSystemParams = await context.SYSTEM_PARAM.ToListWithNoLockAsync(w => lstParams.Contains((w.PARAM_CODE ?? string.Empty).ToUpper()), cancellationToken);

                isValid = lstSystemParams.Any();
            }

            return isValid;
        }
    }
}
