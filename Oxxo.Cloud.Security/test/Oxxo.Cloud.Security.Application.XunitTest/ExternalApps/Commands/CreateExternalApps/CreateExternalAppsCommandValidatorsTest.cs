#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-08.
// Comment: Class Validator of External Apps.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.CreateExternalApps
{
    public class CreateExternalAppsCommandValidatorsTest : AbstractValidator<CreateExternalAppsCommand>
    {
        public CreateExternalAppsCommandValidatorsTest()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(GlobalConstantMessages.VALIDATION_NAME_EMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(GlobalConstantMessages.VALIDATION_CODE_EMPTY).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="code">Code ExternalAplication</param>
        /// <param name="name">Name ExternalAplication</param>
        [Theory]
        [InlineData("", "")]
        [InlineData("", "Description app")]
        [InlineData("CODEAPP", "")]
        [InlineData("CODEAPP", "Description app")]
        public void CreatePermissionsCommand_ValidateNullFields(string code, string name)
        {
            // Arrange
            List<string> lstErrores = new List<string>()
            {
                GlobalConstantMessages.VALIDATION_CODE_EMPTY,
                GlobalConstantMessages.VALIDATION_NAME_EMPTY
            };

            CreateExternalAppsCommand command = new()
            {
                Code = code,
                Name = name
            };

            CreateExternalAppsCommandValidatorsTest validator = new CreateExternalAppsCommandValidatorsTest();

            // Act
            var result = validator.Validate(command);

            // Assert
            if (result.Errors.Count > 0)
            {
                Assert.True(result.Errors[0].ErrorCode == "400" && lstErrores.Any(a => a == result.Errors[0].ErrorMessage));
            }
            else
            {
                Assert.NotNull(result);
            }
        }
    }
}