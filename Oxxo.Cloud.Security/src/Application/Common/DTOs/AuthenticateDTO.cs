#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Dto authenticate.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs;
public class AuthenticateDto : BasePropertiesDto
{
    public string? TypeAuth { get; set; }
}
