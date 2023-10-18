using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesQueryValidatorTest : AbstractValidator<RolesQuery>
    {
        public RolesQueryValidatorTest()
        {

            RuleFor(x => x.Skip).GreaterThanOrEqualTo(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYGETSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Take).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYGETTAKE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="shortName">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>        
        [Theory]
        [InlineData(-2, 3)]
        [InlineData(0, 0)]
        [InlineData(1, -3)]
        [InlineData(null, null)]
        public void GetRoles_ValidateIncorrectFields(int skip, int take)
        {
            RolesQuery query = new()
            {
                Skip = skip,
                Take = take
            };

            var validator = new RolesQueryValidatorTest();

            var result = validator.Validate(query);
            Assert.False(result.IsValid);
        }
    }
}
