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
    public class PermissionsQueryByIdRolValidatorTest : AbstractValidator<PermissionsQueryByIdRol>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public PermissionsQueryByIdRolValidatorTest()
        {
            RuleFor(x => x.roleId)
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
        [InlineData(0, GlobalConstantMessages.MAXVALUEFIELDMESSAGE, "role Id")]
        public void PermissionsQueryByIdRol_ValidateField(int roleId, string messageErrror, string field)
        {
            #region [SET VALUES]
            PermissionsQueryByIdRol command = new()
            {
                roleId = roleId
            };

            var validator = new PermissionsQueryByIdRolValidatorTest();
            #endregion

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400" && string.Format(messageErrror, field) == result.Errors[0].ErrorMessage);
        }
    }
}
