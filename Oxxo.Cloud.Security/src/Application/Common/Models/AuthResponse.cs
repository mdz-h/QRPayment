#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Model of AuthResponse.
//===============================================================================
#endregion
using System.Text.Json.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.Models;

public class AuthResponse
{
    public AuthResponse()
    {
        Token = string.Empty;
        RefreshToken = string.Empty;       
    }

    public bool Auth { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RefreshToken { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? Exp { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IdGuid { get; set; }
}
