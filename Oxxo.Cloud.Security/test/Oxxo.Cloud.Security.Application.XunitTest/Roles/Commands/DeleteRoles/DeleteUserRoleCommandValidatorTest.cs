using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles
{
    public class DeleteUserRoleCommandValidatorTest : AbstractValidator<DeleteUserRoleCommand>
    {
        public DeleteUserRoleCommandValidatorTest()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(GlobalConstantMessages.USERWORKGROUPLINKEMPTYGUID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData(null)]
        public void DeleteUserRoleCommand_ValidateNullUserId(string userId)
        {
            DeleteUserRoleCommand command = new()
            {
                UserId = userId,
            };

            var validator = new DeleteUserRoleCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData("")]
        public void DeleteUserRoleCommand_ValidateUserId(string userId)
        {

            DeleteUserRoleCommand command = new()
            {
                UserId = userId
            };

            var validator = new DeleteUserRoleCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400");
        }
    }
}
