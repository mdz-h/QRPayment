using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.UpdatePermissions
{
    public class UpdatePermissionsCommandValidatorTest : AbstractValidator<UpdatePermissionsCommand>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        public UpdatePermissionsCommandValidatorTest()
        {
            RuleFor(x => x.PermissionID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONCODE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .MaximumLength(200).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 200, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.ModuleID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONMODULEID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.IDVALUEINCORRECT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.PermissionTypeID)
                .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONTYPE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .GreaterThan(0).WithMessage(GlobalConstantMessages.IDVALUEINCORRECT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }

        /// <summary>
        /// This test validate if the fields are not nulls
        /// </summary>
        /// <param name="PermissionID">Permission ID</param>
        /// <param name="Name">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>
        /// <param name="moduleId">Module id of the permission</param>
        /// <param name="permissionTypeID">Permission type</param>
        [Theory]
        [InlineData(0, "permission/creates", "permission/creates", "permissions mocks", 1, 1, GlobalConstantMessages.PERMISSIONID)]
        [InlineData(9, "", "permission/creates", "permissions mocks", 1, 1, GlobalConstantMessages.PERMISSIONNAME)]
        [InlineData(9, "permission/creates", "", "permissions mocks", 1, 1, GlobalConstantMessages.PERMISSIONCODE)]
        [InlineData(9, "permission/creates", "permissions mocks", "", 1, 1, GlobalConstantMessages.PERMISSIONDESCRIPTION)]
        [InlineData(9, "permission/creates", "permissions mocks", "permission/creates", 0, 1, GlobalConstantMessages.PERMISSIONMODULEID)]
        [InlineData(9, "permission/creates", "permissions mocks", "permission/creates", 1, 0, GlobalConstantMessages.PERMISSIONTYPE)]
        [InlineData(9, "permission/creates", "permissions mocks", "permission/creates", -1, 1, GlobalConstantMessages.IDVALUEINCORRECT)]
        [InlineData(9, "permission/creates", "permissions mocks", "permission/creates", 1, -1, GlobalConstantMessages.IDVALUEINCORRECT)]
        public void UpdatePermissionsCommand_ValidateNullFields(int PermissionID, string Name, string code, string description, int moduleId, int permissionTypeID, string msgvalidate)
        {
            #region [SET VALUES]
            UpdatePermissionsCommand command = new()
            {
                PermissionID = PermissionID,
                Name = Name,
                Code = code,
                Description = description,
                ModuleID = moduleId,
                PermissionTypeID = permissionTypeID
            };

            var validator = new UpdatePermissionsCommandValidatorTest();
            #endregion

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400" && msgvalidate == result.Errors[0].ErrorMessage);
        }

        public class UpdatePermissionsCommandValidatorMaxLengthTest : AbstractValidator<UpdatePermissionsCommand>
        {
            public UpdatePermissionsCommandValidatorMaxLengthTest()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONNAME).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                    .MaximumLength(5).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 5, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

                RuleFor(x => x.Code)
                    .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONCODE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                    .MaximumLength(5).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 5, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONDESCRIPTION).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                    .MaximumLength(5).WithMessage(string.Format(GlobalConstantMessages.MAXLENGTHERROR, 5, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

                RuleFor(x => x.ModuleID)
                    .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONMODULEID).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                    .GreaterThan(0).WithMessage(GlobalConstantMessages.IDVALUEINCORRECT).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

                RuleFor(x => x.PermissionTypeID)
                    .NotEmpty().WithMessage(GlobalConstantMessages.PERMISSIONTYPE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            }

            /// <summary>
            /// This test validate maxlength fields
            /// </summary>
            /// <param name="Name">Short name permission</param>
            /// <param name="code">Code Permission</param>
            /// <param name="description">Description permission</param>
            [Theory]
            [InlineData("123456", "12345", "12345", 1, 1, "Name")]
            [InlineData("12345", "123456", "12345", 1, 1, "Code")]
            [InlineData("12345", "12345", "123456", 1, 1, "Description")]
            public void CreatePermissionsCommand_ValidateMaxLength(string Name, string code, string description, int moduleId, int permissionTypeID, string field)
            {
                #region [SET VALUES]
                UpdatePermissionsCommand command = new()
                {
                    Name = Name,
                    Code = code,
                    Description = description,
                    ModuleID = moduleId,
                    PermissionTypeID = permissionTypeID
                };

                var validator = new UpdatePermissionsCommandValidatorMaxLengthTest();
                #endregion

                var result = validator.Validate(command);
                string msg = string.Format(GlobalConstantMessages.MAXLENGTHERROR, 5, field);
                Assert.True(result.Errors[0].ErrorCode == "400" && msg == result.Errors[0].ErrorMessage);
            }
        }
    }
}
