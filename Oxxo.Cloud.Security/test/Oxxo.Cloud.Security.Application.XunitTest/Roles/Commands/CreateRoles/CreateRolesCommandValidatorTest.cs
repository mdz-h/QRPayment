using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.CreateRoles
{
    public class CreateRolesCommandValidatorTest : AbstractValidator<CreateRolesCommand>
    {
        public CreateRolesCommandValidatorTest()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYCODE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.ShortName).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYSHORTNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Description).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData("", "DEVICE_TILL5", "DEVICE TILL DESCRIPTION")]
        [InlineData("DEVICE_TILL5", "", "DEVICE TILL DESCRIPTION")]
        [InlineData("DEVICE_TILL5", "DEVICE_TILL5", "")]
        public void CreateRolesCommand_ValidateNullFields(string shortName, string code, string description)
        {
            CreateRolesCommand command = new()
            {
                ShortName = shortName,
                Code = code,
                Description = description
            };

            var validator = new CreateRolesCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }
    }
}
