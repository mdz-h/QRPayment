#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Interface token generator.
//===============================================================================
#endregion
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces;
public interface ITokenGenerator
{
    public JwtSecurityToken GenerateJWTToken(DeviceTokenDto deviceToken);
    public JwtSecurityToken GenerateJWTToken(string nameApplication, string codeApplication, string timeExpirationMinutes, string externalApplicationId);
    public JwtSecurityToken GenerateJWTToken(int personId, Guid operadorId, string userName, int timeExpirationMinutes, string crPlace, string crStore, string till, string fullName);
    public string GenerateRefreshToken();
    public AuthResponse GetNameIdClaimsToken(string token);
    public UserDto GetUserClaims(string token);
}
