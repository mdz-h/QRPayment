#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-26.
// Comment: Interface IEmail
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Common.DTOs;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface IEmail
    {
        Task<bool> SendEmail(EmailDto email, string token);
    }
}
