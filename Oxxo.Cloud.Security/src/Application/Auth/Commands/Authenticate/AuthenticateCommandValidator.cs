#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Validator authenticate device/external/user.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate;

public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
{
    private readonly IAuthenticateQuery authenticateQuery;
    private readonly ICryptographyService cryptographyService;
    private bool validate = false;

    /// <summary>
    /// Constructor that injects the interface token and cryptography. 
    /// Contains the rules for validation data.
    /// </summary>
    /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
    /// <param name="cryptographyService">Inject the interface with the necessary methods to encrypt and decrypt</param>
    public AuthenticateCommandValidator(IAuthenticateQuery authenticateQuery, ICryptographyService cryptographyService)
    {
        this.authenticateQuery = authenticateQuery;

        this.cryptographyService = cryptographyService;
        RuleFor(v => v.TypeAuth).NotEmpty().WithMessage(GlobalConstantMessages.TYPEAUTHENTICATENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .Must(ValidateTypeAuth).WithMessage(GlobalConstantMessages.TYPEAUTHNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        
        When(v => !string.IsNullOrEmpty(v.TypeAuth) && v.TypeAuth.Equals(GlobalConstantHelpers.DEVICE), () =>
        {
            RuleFor(v => v.DeviceKey)
            .NotEmpty().WithMessage(GlobalConstantMessages.DEVICEKEYNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Id)
            .NotEmpty().WithMessage(GlobalConstantMessages.DEVICENAMENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .MustAsync(ValidateNameDevice).WithMessage(GlobalConstantMessages.DEVICENAMENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString())
            .MustAsync(ValidateDataDevice).WithMessage(GlobalConstantMessages.VALIDATEDATADEVICE).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v)
            .MustAsync(ValidateDeviceKey).WithMessage(GlobalConstantMessages.DEVICEKEYNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

        });

        When(v => !string.IsNullOrEmpty(v.TypeAuth) && v.TypeAuth.Equals(GlobalConstantHelpers.EXTERNAL), () =>
        {
            RuleFor(v => v.Id)
           .NotEmpty().WithMessage(GlobalConstantMessages.CODEEXTERNALNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
           .MustAsync(ValidateCodeExternal).WithMessage(GlobalConstantMessages.CODEEXTERNALNOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());

            RuleFor(v => v.ApiKey)
           .NotEmpty().WithMessage(GlobalConstantMessages.APIKEYEXTERNALNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v)
           .MustAsync(ValidateApiKeyExternal).WithMessage(GlobalConstantMessages.APIKEYEXISTACTIVE).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        });

        When(v => !string.IsNullOrEmpty(v.TypeAuth) && v.TypeAuth.Equals(GlobalConstantHelpers.USER), () =>
        {
            RuleFor(v => v.User)
           .NotEmpty().WithMessage(GlobalConstantMessages.USERNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Password)
           .NotEmpty().WithMessage(GlobalConstantMessages.USERPASSWORDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v)
           .MustAsync(ValidateExistUser).WithMessage(GlobalConstantMessages.USERPASSWORDEXISTACTIVE).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        });

        When(v => !string.IsNullOrEmpty(v.TypeAuth) && v.TypeAuth.Equals(GlobalConstantHelpers.USERXPOS), () =>
        {
            RuleFor(v => v.User)
           .NotEmpty().WithMessage(GlobalConstantMessages.USERNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Password)
           .NotEmpty().WithMessage(GlobalConstantMessages.USERPASSWORDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.CrPlace)
            .NotEmpty().WithMessage(GlobalConstantMessages.CRPLACENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.CrStore)
            .NotEmpty().WithMessage(GlobalConstantMessages.CRSTORENOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.Till)
            .NotEmpty().WithMessage(GlobalConstantMessages.TILLNUMBERNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
            .NotEqual("0").WithMessage(GlobalConstantMessages.TILLNUMBERNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v)
            .MustAsync(ValidateExistStoreAndPlace).WithMessage(GlobalConstantMessages.USERCRPlACEANDCRSTORENOTFOUND).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
        });


        When(v => !string.IsNullOrEmpty(v.TypeAuth) && v.TypeAuth.Equals(GlobalConstantHelpers.USERXPOS) && !v.IsInternal, () =>
        { 
            RuleFor(v => v.AdministrativeDate)
            .NotEmpty().WithMessage(GlobalConstantMessages.USERADMINISTRATIVEDATE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.ProcessDate)
            .NotEmpty().WithMessage(GlobalConstantMessages.USERPASSWORDNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.WorkGroup)
          .NotEmpty().WithMessage(GlobalConstantMessages.USERWORKGROUPNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.StatusOperator)
          .NotEmpty().WithMessage(GlobalConstantMessages.USERSTATUSOPERATORNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(v => v.FullName)
         .NotEmpty().WithMessage(GlobalConstantMessages.FULLNAMEOPERATORNOTEMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

        });

        RuleFor(v => v.Identification).NotEmpty().WithMessage(GlobalConstantMessages.IDENTIFICATIONOPERATORDEFUALT).WithErrorCode(((int)HttpStatusCode.UnprocessableEntity).ToString());
    }   

    private bool ValidateTypeAuth(string typeAuth)
    {
        if (typeAuth == GlobalConstantHelpers.DEVICE || typeAuth == GlobalConstantHelpers.EXTERNAL || typeAuth == GlobalConstantHelpers.USER || typeAuth == GlobalConstantHelpers.USERXPOS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Validator Device

    private async Task<bool> ValidateNameDevice(string? nameDevice, CancellationToken cancellationToken)
    {
        return validate = await authenticateQuery.ValidateNameDevice(nameDevice);
    }

    private async Task<bool> ValidateDataDevice(string? nameDevice, CancellationToken cancellationToken)
    {
        if (!validate)
        {
            return true;
        }
        return validate = await authenticateQuery.ValidateDataDevice(nameDevice);
    }

    private async Task<bool> ValidateDeviceKey(AuthenticateCommand authenticate, CancellationToken cancellationToken)
    {
        if (!validate)
        {
            return true;
        }
        var device = await authenticateQuery.GetDeviceToken(authenticate.Id);
        if (device == null)
        {
            return validate = false;
        }
        var deviceKey = string.Concat("{", device.CrPlace, "}-{", device.CrStore, "}-{", device.NumberDevice, "}-{", device.MacAddress, "}-{", device.IP, "}-{", device.IdProcessor, "}-{", device.NetworkCard, "}");

        if (cryptographyService.EncryptHash(deviceKey) == authenticate.DeviceKey)
        {
            return validate = true;
        }
        else
        {
            return validate = false;
        }

    }

    #endregion

    #region Validator External
    private async Task<bool> ValidateCodeExternal(string code, CancellationToken cancellationToken)
    {
        return validate = await authenticateQuery.ValidateCodeExternal(code);
    }

    private async Task<bool> ValidateApiKeyExternal(AuthenticateCommand authenticate, CancellationToken cancellationToken)
    {
        if (!validate)
        {
            return true;
        }
        var apiKeyExternalApplication = await authenticateQuery.GetApiKeyExternalApplication(authenticate.Id, authenticate.ApiKey);
        if (apiKeyExternalApplication == null || string.IsNullOrEmpty(apiKeyExternalApplication))
        {
            return false;
        }
        var apiKeyDecrypted = cryptographyService.Decrypt(apiKeyExternalApplication);
        if (apiKeyDecrypted == authenticate.ApiKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region User
    private async Task<bool> ValidateExistUser(AuthenticateCommand authenticate, CancellationToken cancellationToken)
    {
        return await authenticateQuery.ValidateExistUserPassword(authenticate.User, authenticate.Password);
    }

    private async Task<bool> ValidateExistStoreAndPlace(AuthenticateCommand authenticate, CancellationToken cancellationToken)
    {
        return await authenticateQuery.ValidateExistAndActiveStorePlace(authenticate.CrStore, authenticate.CrPlace, cancellationToken);
    }

    #endregion
}