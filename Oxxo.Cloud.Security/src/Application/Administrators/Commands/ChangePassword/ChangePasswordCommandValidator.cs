#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Validator of Command for Change password Administrators.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    /// <summary>
    /// Principal class to validate params
    /// </summary>
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly IChangePassword password;
        private bool validate = false;

        /// <summary>
        /// Constructor ChangePasswordCommandValidator that injects the interface IChangePassword. 
        /// </summary>                
        /// <param name="password"></param>
        public ChangePasswordCommandValidator(IChangePassword password)
        {
            this.password = password;
            RuleFor(request => request.Token)
                .NotEmpty().WithMessage(GlobalConstantMessages.TOKENNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(request => request.UserId)
              .NotEmpty().WithMessage(GlobalConstantMessages.VALIDATEDATAUSERID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(request => request.OldPassword)
              .NotEmpty().WithMessage(GlobalConstantMessages.VALIDATEDATAOLDPASSWORD).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(request => request.NewPassword)
              .NotEmpty().WithMessage(GlobalConstantMessages.VALIDATEDATANEWPASSWORD).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(request => request)
           .Must(ValidateTypeUserId).WithMessage(GlobalConstantMessages.VALIDATETYPEUSERID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
           .MustAsync(ValidateIfExistsAdministratorAsync).WithMessage(GlobalConstantMessages.VALIDATEIFEXISTSADMINISTRATOR).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
           .MustAsync(ValidateIfPasswordIsEqualToTheCurrentPassword).WithMessage(GlobalConstantMessages.VALIDATEPASSWORDISEQUALTOTHECURRENTPASSWORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
           .MustAsync(ValidateNewPassword).WithMessage(GlobalConstantMessages.VALIDATENEWPASSWORD).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
           .MustAsync(ValidateNewPasswordWithTheLastPasswords).WithMessage(GlobalConstantMessages.VALIDATELASTPASSWORDS).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        }

        /// <summary>
        /// This function is responsible that validate the value type on field UserId
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private bool ValidateTypeUserId(ChangePasswordCommand request)
        {          
            return this.validate = Guid.TryParse(request.UserId, out Guid guidOutput);
        }

        /// <summary>
        /// This function is responsible that invoke the for validating query if exists a Administrator
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private async Task<bool> ValidateIfExistsAdministratorAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }
            return validate = await password.ValidateExistsAdministratorAsync(request, cancellationToken);
        }

        /// <summary>
        /// This function is responsible that invoke the query for validating if password is equal to the current password
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private async Task<bool> ValidateIfPasswordIsEqualToTheCurrentPassword(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }
            return validate = await password.ValidateIfPasswordIsEqualToTheCurrentPassword(request, cancellationToken);
        }

        /// <summary>
        /// This function is responsible that invoke the query for validating that the new password complies with all the rules.
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private async Task<bool> ValidateNewPassword(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!validate || request.NewPassword == string.Empty)
            {
                return true;
            }
            return validate = await password.ValidateNewPassword(request, cancellationToken);
        }

        /// <summary>
        /// This function is responsible that invoke the query for validating if the new password not is equal in the last passwords
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private async Task<bool> ValidateNewPasswordWithTheLastPasswords(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!validate)
            {
                return true;
            }
            return validate = await password.ValidateNewPasswordWithTheLastPasswords(request, cancellationToken);
        }
    }
}
