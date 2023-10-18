using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteRolesCommandValidatorTest : AbstractValidator<DeleteRolesCommand>
    {
        public DeleteRolesCommandValidatorTest()
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData(-2)]
        [InlineData(0)]
        [InlineData(null)]
        public void DeleteRolesCommand_ValidateValidRoleId(int roleId)
        {
            DeleteRolesCommand command = new()
            {
                RoleId = roleId
            };

            var validator = new DeleteRolesCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

    }
}
