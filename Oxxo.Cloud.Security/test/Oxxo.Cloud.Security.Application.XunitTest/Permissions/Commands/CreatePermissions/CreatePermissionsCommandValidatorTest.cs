using FluentValidation;
using FluentValidation.TestHelper;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Permissions.Commands.CreatePermissions
{
    public class CreatePermissionsCommandValidatorTest : AbstractValidator<CreatePermissionsCommand>
    {
        public CreatePermissionsCommandValidatorTest()
        {
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
        /// <param name="Name">Short name permission</param>
        /// <param name="code">Code Permission</param>
        /// <param name="description">Description permission</param>
        /// <param name="moduleId">Module id of the permission</param>
        /// <param name="permissionTypeID">Permission type</param>
        [Theory]
        [InlineData("", "permission/creates", "permissions mocks", 1, 1)]
        [InlineData("permission/creates", "", "permissions mocks", 1, 1)]
        [InlineData("permission/creates", "permissions mocks", "", 1, 1)]
        [InlineData("permission/creates", "permissions mocks", "permission/creates", 0, 1)]
        [InlineData("permission/creates", "permissions mocks", "permission/creates", 1, 0)]
        [InlineData("permission/creates", "permissions mocks", "permission/creates", -1, 1)]
        [InlineData("permission/creates", "permissions mocks", "permission/creates", 1, -1)]
        public void CreatePermissionsCommand_ValidateNullFields(string Name, string code, string description, int moduleId, int permissionTypeID)
        {
            List<string> lstErrores = new List<string>()
            {
                GlobalConstantMessages.PERMISSIONNAME,
                GlobalConstantMessages.PERMISSIONDESCRIPTION,
                GlobalConstantMessages.PERMISSIONTYPE,
                GlobalConstantMessages.PERMISSIONID,
                GlobalConstantMessages.PERMISSIONCODE,
                GlobalConstantMessages.PERMISSIONMODULEID,
                GlobalConstantMessages.IDVALUEINCORRECT
            };

            CreatePermissionsCommand command = new()
            {
                Name = Name,
                Code = code,
                Description = description,
                ModuleID = moduleId,
                PermissionTypeID = permissionTypeID
            };

            var validator = new CreatePermissionsCommandValidatorTest();

            var result = validator.Validate(command);
            Assert.True(result.Errors[0].ErrorCode == "400" && lstErrores.Any(a => a == result.Errors[0].ErrorMessage));
        }
    }

    public class CreatePermissionsCommandValidatorMaxLengthTest : AbstractValidator<CreatePermissionsCommand>
    {
        public CreatePermissionsCommandValidatorMaxLengthTest()
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
            CreatePermissionsCommand command = new()
            {
                Name = Name,
                Code = code,
                Description = description,
                ModuleID = moduleId,
                PermissionTypeID = permissionTypeID
            };

            var validator = new CreatePermissionsCommandValidatorMaxLengthTest();

            var result = validator.Validate(command);
            string msg = string.Format(GlobalConstantMessages.MAXLENGTHERROR, 5, field);
            Assert.True(result.Errors[0].ErrorCode == "400" && msg == result.Errors[0].ErrorMessage);
        }
    }
}
