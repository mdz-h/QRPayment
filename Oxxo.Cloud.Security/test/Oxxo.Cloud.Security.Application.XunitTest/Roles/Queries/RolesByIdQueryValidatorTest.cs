using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesByIdQueryValidatorTest : AbstractValidator<RolesByIdQuery>
    {
        public RolesByIdQueryValidatorTest()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage(GlobalConstantMessages.WORKGROUPEMPTYID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString()).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPIDNEGATIVE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
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
        public void GetRolesWithId_ValidateIncorrectFields(int roleId)
        {
            RolesByIdQuery query = new()
            {
                RoleId = roleId
            };

            var validator = new RolesByIdQueryValidatorTest();

            var result = validator.Validate(query);
            Assert.False(result.IsValid);
        }
    }
}

