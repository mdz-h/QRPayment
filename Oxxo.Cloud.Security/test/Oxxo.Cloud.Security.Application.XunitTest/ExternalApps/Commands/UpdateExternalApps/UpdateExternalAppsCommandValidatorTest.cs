using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.UpdateExternalApps
{
    public class UpdateExternalAppsCommandValidatorTest : AbstractValidator<UpdateExternalAppsCommand>
    {
        public UpdateExternalAppsCommandValidatorTest()
        {
            RuleFor(x => x.ExternalAppId).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString()).NotNull().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Name).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(100).WithMessage(GlobalConstantMessages.EXTERNALAPPSLENGTHNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.IsActive).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYISACTIVE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the id fields are not nulls or empty
        /// </summary>
        /// <param name="externalAppId">id for external application</param>
        /// <param name="name">name for external application</param>
        /// <param name="isActive">status for external application</param>        
        [Theory]
        [InlineData("", "OTHER NAME TEST", true)]
        [InlineData(null, "OTHER NAME TEST", true)]
        public void ValidateInvalidExternalAppId(string externalAppId, string name, bool isActive)
        {
            UpdateExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive
            };
            var validator = new UpdateExternalAppsCommandValidatorTest();
            var result = validator.Validate(command);
            Assert.True(!result.IsValid);
            Assert.Equal("External application Id: not empty allowed.", result.Errors[0].ErrorMessage);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

        /// <summary>
        /// This test validate if the name fields are not nulls or empty
        /// </summary>
        /// <param name="externalAppId">id for external application</param>
        /// <param name="name">name for external application</param>
        /// <param name="isActive">status for external application</param>        
        [Theory]
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4", "", true)]
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4", null, true)]
        public void ValidateInvalidExternalAppName(string externalAppId, string name, bool isActive)
        {
            UpdateExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive
            };
            var validator = new UpdateExternalAppsCommandValidatorTest();
            var result = validator.Validate(command);
            Assert.True(!result.IsValid);
            Assert.Equal("Name: not empty allowed.", result.Errors[0].ErrorMessage);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

        /// <summary>
        /// This test validate if the name fields are not nulls or empty
        /// </summary>
        /// <param name="externalAppId">id for external application</param>
        /// <param name="name">name for external application</param>
        /// <param name="isActive">status for external application</param>        
        [Theory]
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4", "This_is_a_text_with_many_words_whose_goal_is_fail_this_test_validating_the_rule_of_maximum_length_at_this_field", true)]
        public void ValidateMaxLengthOfExternalAppName(string externalAppId, string name, bool isActive)
        {
            UpdateExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive
            };
            var validator = new UpdateExternalAppsCommandValidatorTest();
            var result = validator.Validate(command);
            Assert.True(!result.IsValid);
            Assert.Equal("Name: Field length has been exceeded.", result.Errors[0].ErrorMessage);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

        /// <summary>
        /// This test validate if the name fields are not nulls or empty
        /// </summary>
        /// <param name="externalAppId">id for external application</param>
        /// <param name="name">name for external application</param>
        /// <param name="isActive">status for external application</param>        
        [Theory]
        [InlineData("59231A3F-155C-4064-8293-FF299735A6E4", "This_is_a_text", null)]
        public void ValidateExternalAppIsActive(string externalAppId, string name, bool? isActive)
        {
            UpdateExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId,
                Name = name,
                IsActive = isActive
            };
            var validator = new UpdateExternalAppsCommandValidatorTest();
            var result = validator.Validate(command);
            Assert.NotNull(result);
        }
    }
}
