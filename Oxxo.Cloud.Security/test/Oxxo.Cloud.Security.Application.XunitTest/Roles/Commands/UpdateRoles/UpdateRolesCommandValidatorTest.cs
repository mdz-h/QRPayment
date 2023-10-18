using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.UpdateRoles
{
    public class UpdateRolesCommandValidatorTest : AbstractValidator<UpdateRolesCommand>
    {
        public UpdateRolesCommandValidatorTest()
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
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
        [InlineData(0, "DEVICE_TILL", true, "DEVICE TILL DESCRIPTION")]
        [InlineData(-4, "DEVICE_TILL5", true, "DEVICE TILL DESCRIPTION")]
        public void UpdateRolesCommand_ValidateEmptyFields(int roleId, string shortName, bool isActive, string description)
        {
            UpdateRolesCommand command = new()
            {
                RoleId = roleId,
                ShortName = shortName,
                IsActive = isActive,
                Description = description
            };

            var validator = new UpdateRolesCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.IsValid == false);
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData(-2, "CODE_TEST", true, "DEVICE TILL DESCRIPTION")]
        [InlineData(0, "CODE_TEST", false, "DEVICE TILL DESCRIPTION")]
        [InlineData(null, "CODE_TEST", true, "DEVICE TILL DESCRIPTION")]
        public void UpdateRolesCommand_ValidateValidRoleId(int roleId, string shortName, bool isActive, string description)
        {
            UpdateRolesCommand command = new()
            {
                RoleId = roleId,
                ShortName = shortName,
                IsActive = isActive,
                Description = description
            };

            var validator = new UpdateRolesCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

    }
}
