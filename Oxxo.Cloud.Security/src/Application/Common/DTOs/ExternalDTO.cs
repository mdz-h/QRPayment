#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Dto external.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.DTOs;
public class ExternalDto : AuthenticateDto
{
    public string? Id { get; set; }

    public string? ApiKey { get; set; }
}
