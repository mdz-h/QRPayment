#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Interface authenticate query.
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces;
public interface IAuthenticateQuery
{
    Task<bool> ValidateNameDevice(string? nameDevice);
    Task<bool> ValidateSessionActiveDevice(string nameDevice);
    Task<bool> ValidateSessionActive(Guid idGuid);
    Task<bool> ValidateDataDevice(string? nameDevice);
    Task<bool> ValidateCodeExternal(string code);
    Task<ExternalApplication?> GetExternalApplication(Guid guidId, CancellationToken cancellationToken);
    Task<bool> ValidateToken(string token);
    Task<bool> ValidatePermissionToken(string? token, string endpoint);
    Task<bool> ValidateExpirationToken(string? token, CancellationToken cancellationToken);
    Task<bool> ValidateExistRefreshToken(string? refreshToken);
    Task<string?> GetRefreshToken(string? token);
    Task<string?> GetApiKeyExternalApplication(string? codeApplication, string ApiKey);
    Task<DeviceTokenDto?> GetDeviceToken(Guid deviceId);
    Task<DeviceTokenDto?> GetDeviceToken(string nameDevice);
    Task<AuthResponse> GetSessionTokenDevice(string nameDevice);
    Task<AuthResponse> GetSessionToken(Guid guidId);
    Task<bool> ValidateExistUserPassword(string user, string password);
    Task<SystemParam?> GetTimeExpirationTokenUser(CancellationToken cancellationToken);
    Task<Operator> GetOperatorActiveForUserNotNull(string userName, CancellationToken cancellationToken);
    Task<Operator?> GetOperatorActiveForGuidId(Guid operadorId, CancellationToken cancellationToken);
    Task<bool> ValidateExistAndActiveStorePlace(string crStore, string crPlace, CancellationToken cancellationToken);
    Task<Operator?> GetOperatorActiveForUser(string userName, CancellationToken cancellationToken);
    Task<List<SystemParam>> GetSystemParamsPasswordRules(CancellationToken cancellationToken);
    Task<Operator?> GetOperatorActiveForUserPassword(string userName, string password, CancellationToken cancellationToken);
    Task<OperatorStatus> GetOperadtorStatus(string codeStatus, CancellationToken cancellationToken);
}
