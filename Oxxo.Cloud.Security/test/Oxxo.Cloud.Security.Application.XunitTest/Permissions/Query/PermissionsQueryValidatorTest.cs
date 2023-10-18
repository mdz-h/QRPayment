#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    01/12/2022.
// Comment: permissions.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Application.Permissions.Queries;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Query
{
    public class PermissionsQueryValidatorTest : AbstractValidator<PermissionsQuery>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryValidatorTest()
        {
            RuleFor(x => x.PageNumber)
               .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .GreaterThanOrEqualTo(1).WithMessage(string.Format(GlobalConstantMessages.GREATEROREQUALSTHANFIELD, "{PropertyName}", "1")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ItemsNumber)
              .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
              .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls or empty
        /// </summary>
        /// <param name="skip">skip</param>
        /// <param name="take">take</param>
        /// <param name="field">field</param>
        /// <param name="messageErrror">message error to validate</param>
        [Theory]
        /*The value skipe in null be convert to 0 and this test must validate the field take*/
        [InlineData(0, 1, "Page Number", GlobalConstantMessages.GREATEROREQUALSTHANFIELD)]
        /*The value skipe an items Number in null be convert to 0 and this test must validate the field take*/
        [InlineData(0, 0, "Page Number", GlobalConstantMessages.GREATEROREQUALSTHANFIELD)]
        [InlineData(-1, 0, "Page Number", GlobalConstantMessages.GREATEROREQUALSTHANFIELD)]
        [InlineData(1, -2, "Items Number", GlobalConstantMessages.MAXVALUEFIELDMESSAGE)]
        public void PermissionsQuery_ValidateField(int skip, int take, string field, string messageErrror)
        {
            #region [SET VALUES]
            PermissionsQuery command = new()
            {
                PageNumber = skip,
                ItemsNumber = take
            };

            var validator = new PermissionsQueryValidatorTest();
            #endregion

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400" && string.Format(messageErrror, field, 1) == result.Errors[0].ErrorMessage);
        }
    }
}
