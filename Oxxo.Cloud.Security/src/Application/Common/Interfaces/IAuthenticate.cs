#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Interface authenticate.
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Common.Models;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces;
public interface IAuthenticate
{
    Task<AuthResponse> Auth(CancellationToken cancellationToken);
}
