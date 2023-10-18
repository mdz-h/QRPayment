#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Interface IChangePassword
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IChangePassword
    {
        Task<bool> ChangePasswordAdministratorAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
        Task<bool> ValidateExistsAdministratorAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
        Task<bool> ValidateIfPasswordIsEqualToTheCurrentPassword(ChangePasswordCommand request, CancellationToken cancellationToken);
        Task<bool> ValidateNewPassword(ChangePasswordCommand request, CancellationToken cancellationToken);
        Task<bool> ValidateNewPasswordWithTheLastPasswords(ChangePasswordCommand request, CancellationToken cancellationToken);
    }
}
