#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Validator token query.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Queries;
public class ValidateTokenQueryValidator : AbstractValidator<ValidateTokenQuery>
{
    /// <summary>
    /// Constructor that injects the interface token query. 
    /// </summary>    
    public ValidateTokenQueryValidator()
    {
        RuleFor(x => x.Endpoint).NotEmpty().WithMessage(GlobalConstantMessages.ENDPOINTNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        RuleFor(v => v.Token)
        .NotEmpty().WithMessage(GlobalConstantMessages.TOKENNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
        .MinimumLength(GlobalConstantHelpers.MINLENGHTTOKEN).WithMessage(GlobalConstantMessages.TOKENMINLENGHT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
    }
}
