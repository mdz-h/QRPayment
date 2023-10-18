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
    public class PermissionsQueryByIdValidatorTest : AbstractValidator<PermissionsQueryById>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryByIdValidatorTest()
        {
            RuleFor(x => x.permissionID)
               .NotNull().WithMessage(GlobalConstantMessages.PERMISSIONID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .GreaterThan(0).WithMessage(string.Format(GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls or empty
        /// </summary>
        /// <param name="permissionId">Permission ID</param> 
        /// <param name="messageErrror">message error to validate</param>
        /// <param name="field">field</param>
        [Theory]
        [InlineData(0, GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "permission ID")]
        public void PermissionsQueryById_ValidateField(int permissionId, string messageErrror, string field)
        {
            #region [SET VALUES]
            PermissionsQueryById command = new()
            {
                permissionID = permissionId
            };

            var validator = new PermissionsQueryByIdValidatorTest();
            #endregion

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400" && string.Format(messageErrror, field) == result.Errors[0].ErrorMessage);
        }
    }
}
