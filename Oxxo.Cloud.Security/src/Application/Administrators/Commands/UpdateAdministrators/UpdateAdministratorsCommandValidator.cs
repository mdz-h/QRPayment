#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    07/12/2022.
// Comment: Administrator.
//===============================================================================
#endregion

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;
using System.Text.RegularExpressions;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.UpdateAdministrators
{
    /// <summary>
    /// Class validate
    /// </summary>
    public class UpdateAdministratorsCommandValidator : AbstractValidator<UpdateAdministratorsCommand>
    {
        private readonly IApplicationDbContext context;
        private bool isValid = true;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="authenticateQuery">Authenticate</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public UpdateAdministratorsCommandValidator(IApplicationDbContext context)
        {
            this.context = context;

            RuleFor(x => x.UserId)
               .NotEmpty().WithMessage(GlobalConstantMessages.USERIDINVALID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.NickName)
              .NotEmpty().WithMessage(GlobalConstantMessages.NICKNAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.UserName)
              .NotEmpty().WithMessage(GlobalConstantMessages.USERNAMEMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.LastNamePat)
              .NotEmpty().WithMessage(GlobalConstantMessages.LASTNAMEPATNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.LastNameMat)
                .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.MiddleName)
              .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.IsActive)
              .NotNull().WithMessage(string.Format(GlobalConstantMessages.FIELDNULL, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Email)
              .NotEmpty().WithMessage(GlobalConstantMessages.EMAILNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .MaximumLength(100).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 100, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .Must(ValidateEmail).WithMessage(x => string.Format(GlobalConstantMessages.EMAILINVALID, x.Email)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(x => x)
              .MustAsync(ValidateAdministrators).WithMessage(GlobalConstantMessages.NORECORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
              .MustAsync(ValidateActive).WithMessage(x => string.Format(GlobalConstantMessages.ADMINISTRATORSISFALSE, x.UserId)).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// Validate if the administrator was active
        /// </summary>
        /// <param name="request">request parameters</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>logic value</returns>
        private async Task<bool> ValidateActive(UpdateAdministratorsCommand request, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = (request.IsActive ?? false) || await context.OPERATOR.AsNoTracking()
                                                            .Include(i => i.PERSON)
                                                            .AnyAsync(w => w.OPERATOR_ID.ToString() == request.UserId && w.IS_ACTIVE, cancellationToken);
            }

            return isValid;
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
        /// Validate if exists Administrators
        /// </summary>
        /// <param name="administrators">Administrators class</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Return if the validate is successful</returns>
        private async Task<bool> ValidateAdministrators(UpdateAdministratorsCommand administrators, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                isValid = await context.OPERATOR.AsNoTracking().Where(w => w.OPERATOR_ID.ToString() == (administrators.UserId ?? string.Empty).ToString()).AnyAsync(cancellationToken);
            }
            return isValid;
        }
    }
}
