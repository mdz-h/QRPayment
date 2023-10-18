using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteExternalApps
{
    public class DeleteExternalAppsCommandValidatorTest : AbstractValidator<DeleteExternalAppsCommand>
    {
        public DeleteExternalAppsCommandValidatorTest()
        {
            RuleFor(x => x.ExternalAppId).NotEmpty().WithMessage(GlobalConstantMessages.EXTERNALAPPSEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the id fields are not nulls or empty
        /// </summary>
        /// <param name="externalAppId">id for external application</param>
        /// <param name="name">name for external application</param>
        /// <param name="isActive">status for external application</param>        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateInvalidExternalAppId(string externalAppId)
        {
            DeleteExternalAppsCommand command = new()
            {
                ExternalAppId = externalAppId
            };
            var validator = new DeleteExternalAppsCommandValidatorTest();
            var result = validator.Validate(command);
            Assert.True(!result.IsValid);
            Assert.Equal("External application Id: not empty allowed.", result.Errors[0].ErrorMessage);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

    }
}
