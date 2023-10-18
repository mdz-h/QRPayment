using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByUserIdQueryValidatorTest : AbstractValidator<RolesByUserIdQuery>
    {
        public RolesByUserIdQueryValidatorTest()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(GlobalConstantMessages.USERWORKGROUPLINKEMPTYGUID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString()).NotNull().WithMessage(GlobalConstantMessages.USERWORKGROUPLINKEMPTYGUID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetRolesWithId_ValidateIncorrectFields(string userId)
        {
            RolesByUserIdQuery query = new()
            {
                UserId = userId
            };

            var validator = new RolesByUserIdQueryValidatorTest();

            var result = validator.Validate(query);
            Assert.False(result.IsValid);
        }

    }
}
