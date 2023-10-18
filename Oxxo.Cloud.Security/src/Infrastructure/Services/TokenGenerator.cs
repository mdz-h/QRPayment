#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class token generate service.
//===============================================================================
#endregion
using Microsoft.IdentityModel.Tokens;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Oxxo.Cloud.Security.Infrastructure.Services;
public class TokenGenerator : ITokenGenerator
{
    private readonly string key;
    private readonly string issuer;
    private readonly string audience;

    /// <summary>
    /// Assign the values needed for create the token
    /// </summary>
    /// <param name="key">Contains the value the security for create token</param>
    /// <param name="issueer">Contains the issueer for create token</param>
    /// <param name="audience">Contains the audience for create token</param>
    public TokenGenerator(string key, string issueer, string audience)
    {
        this.key = key;
        this.issuer = issueer;
        this.audience = audience;
    }

    /// <summary>
    /// Function that generates the token based on the input data, the security key and the expiration time.
    /// Assign the important data stored in their corresponding claims.
    /// </summary>
    /// <param name="deviceToken">Contains the main values for token generation</param>
    /// <returns>JWT Token Security generated</returns>
    public JwtSecurityToken GenerateJWTToken(DeviceTokenDto deviceToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, deviceToken.CrPlace),
                new Claim(JwtRegisteredClaimNames.Jti, deviceToken.CrStore),
                new Claim(ClaimTypes.Name, deviceToken.Name),
                new Claim(GlobalConstantHelpers.DEVICEKEY, deviceToken.Key),
                new Claim(GlobalConstantHelpers.NUMBERDEVICE, deviceToken.NumberDevice.ToString()),
                new Claim(GlobalConstantHelpers.IDGUID, deviceToken.DeviceId.ToString()),
                new Claim(GlobalConstantHelpers.CRPLACE, deviceToken.CrPlace),
                new Claim(GlobalConstantHelpers.CRSTORE, deviceToken.CrStore),
                new Claim(GlobalConstantHelpers.TILL, deviceToken.NumberDevice.ToString()),
                new Claim(GlobalConstantHelpers.FULLNAME, deviceToken.Name)
        };

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(deviceToken?.TimeTokenExpiration)),
            signingCredentials: signingCredentials
       );
    }

    /// <summary>
    /// Function that generates the token based on the input data, the security key and the expiration time.
    /// Assign the important data stored in their corresponding claims.
    /// </summary>
    /// <param name="nameApplication">The contains the name application external</param>
    /// <param name="codeApplication">The contains the code application external</param>
    /// <param name="timeExpirationMinutes">The contains the time expiration in minutes</param>
    /// <param name="externalApplicationId">The contains the id application extternal</param>
    /// <returns>JWT Token Security generated</returns>
    public JwtSecurityToken GenerateJWTToken(string nameApplication, string codeApplication, string timeExpirationMinutes, string externalApplicationId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,nameApplication),
                new Claim(JwtRegisteredClaimNames.Sub, codeApplication),
                new Claim(GlobalConstantHelpers.IDGUID,externalApplicationId),
                new Claim(GlobalConstantHelpers.FULLNAME, nameApplication)
            };

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(timeExpirationMinutes)),
            signingCredentials: signingCredentials
       );
    }

    /// <summary>
    /// Function that generates the token based on the input data, the security key and the expiration time.
    /// Assign the important data stored in their corresponding claims for user.
    /// </summary>
    /// <param name="personId">Value id int person for user</param>
    /// <param name="operadorId">Value id guid operator for user</param>
    /// <param name="userName">Value name string operator</param>
    /// <param name="timeExpirationMinutes">Value time in minutos to add for expiration token</param>
    /// <returns></returns>
    public JwtSecurityToken GenerateJWTToken(int personId, Guid operadorId, string userName, int timeExpirationMinutes, string crPlace, string crStore, string till, string fullName)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(JwtRegisteredClaimNames.Sub,personId.ToString()),
                new Claim(GlobalConstantHelpers.IDGUID,operadorId.ToString()),
                new Claim(GlobalConstantHelpers.CRPLACE, crPlace),
                new Claim(GlobalConstantHelpers.CRSTORE, crStore),
                new Claim(GlobalConstantHelpers.TILL, till),
                new Claim(GlobalConstantHelpers.FULLNAME, fullName.Replace(GlobalConstantHelpers.CARACTERPAID,GlobalConstantHelpers.CARACTERSPACE))
            };

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(timeExpirationMinutes)),
            signingCredentials: signingCredentials
       );
    }

    /// <summary>
    /// Function that generate the refresh token.
    /// </summary>
    /// <returns>String with refresh token</returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Get claims token for nameId
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public AuthResponse GetNameIdClaimsToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        return new AuthResponse() {
            Name = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value.ToString(),
            IdGuid = jwtSecurityToken.Claims.First(claim => claim.Type.ToString() == GlobalConstantHelpers.IDGUID).Value.ToString()
        };
    }

    public UserDto GetUserClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        return new UserDto()
        {
            CrPlace = !jwtSecurityToken.Claims.Any(claim => claim.Type == GlobalConstantHelpers.CRPLACE) ? string.Empty 
            : jwtSecurityToken.Claims.First(claim => claim.Type == GlobalConstantHelpers.CRPLACE).Value,
            CrStore = !jwtSecurityToken.Claims.Any(claim => claim.Type == GlobalConstantHelpers.CRSTORE) ? string.Empty
            : jwtSecurityToken.Claims.First(claim => claim.Type == GlobalConstantHelpers.CRSTORE).Value,
            Till = !jwtSecurityToken.Claims.Any(claim => claim.Type == GlobalConstantHelpers.TILL) ? string.Empty
            : jwtSecurityToken.Claims.First(claim => claim.Type == GlobalConstantHelpers.TILL).Value,
            FullName = !jwtSecurityToken.Claims.Any(claim => claim.Type == GlobalConstantHelpers.FULLNAME) ? string.Empty
            : jwtSecurityToken.Claims.First(claim => claim.Type == GlobalConstantHelpers.FULLNAME).Value
        };
    }

}
