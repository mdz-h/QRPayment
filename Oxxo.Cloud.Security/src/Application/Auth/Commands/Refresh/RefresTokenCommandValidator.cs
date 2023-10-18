#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Validator refresh token.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;

public class RefresTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    private readonly IAuthenticateQuery authenticateQuery;
    private bool validate = false;

    /// <summary>
    /// Constructor that injects the interface token query. 
    /// </summary>
    /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
    public RefresTokenCommandValidator(IAuthenticateQuery authenticateQuery)
    {
        this.authenticateQuery = authenticateQuery;

        RuleFor(v => v.Token)
        .NotEmpty().WithMessage(GlobalConstantMessages.TOKENNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
        .MinimumLength(GlobalConstantHelpers.MINLENGHTTOKEN).WithMessage(GlobalConstantMessages.TOKENMINLENGHT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
        .MustAsync(ValidateExistToken).WithMessage(GlobalConstantMessages.TOKENMINLENGHT).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

        RuleFor(v => v.RefreshToken)
        .NotEmpty().WithMessage(GlobalConstantMessages.REFRESHTOKENEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
        .MinimumLength(GlobalConstantHelpers.MINLENGHTREFRESHTOKEN).WithMessage(GlobalConstantMessages.REFRESHTOKENINVALID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
        .MustAsync(ValidateExistRefreshToken).WithMessage(GlobalConstantMessages.VALIDATEEXISTREFRESHTOKEN).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

        RuleFor(x => x).MustAsync(ValidateCorrespondsRefreshToken).WithMessage(GlobalConstantMessages.VALIDATECORRESPONDSREFRESHTOKEN).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        RuleFor(v => v.Identification).NotEmpty().WithMessage(GlobalConstantMessages.IDENTIFICATIONOPERATORDEFUALT).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
    }

    private async Task<bool> ValidateExistToken(string token, CancellationToken cancellationToken)
    {
        return validate = await authenticateQuery.ValidateToken(token);
    }

    private async Task<bool> ValidateExistRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        if (validate)
        {
            return await authenticateQuery.ValidateExistRefreshToken(refreshToken);
        }
        else
        {
            return true;
        }
    }

    private async Task<bool> ValidateCorrespondsRefreshToken(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!validate)
        {
            return true;
        }

        var refreshToken = await authenticateQuery.GetRefreshToken(request.Token);
        if (refreshToken == request.RefreshToken)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
