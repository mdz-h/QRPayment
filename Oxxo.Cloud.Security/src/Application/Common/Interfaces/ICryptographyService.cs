#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Interface cryptography service.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Application.Common.Interfaces;
public interface ICryptographyService
{
    string Encrypt(string text);
    string Decrypt(string base64Text);
    string EncryptHash(string text);
}
