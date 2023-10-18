#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class cryptography service.
//===============================================================================
#endregion
using Oxxo.Cloud.RSAEncryption.Application.Cryptography.Commands.CreateDecrypt;
using Oxxo.Cloud.RSAEncryption.Application.Cryptography.Commands.CreateEncrypt;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Oxxo.Cloud.Security.Infrastructure.Services;
public class CryptographyService : ICryptographyService
{
    private readonly string publicKey;
    private readonly string privateKey;

    /// <summary>
    /// Assign the values of the public and private keys
    /// </summary>
    /// <param name="publicKey">Contains the value public string for encrypt</param>
    /// <param name="privateKey">Contains the value private string for decrypt</param>
    public CryptographyService(string publicKey, string privateKey)
    {
        this.publicKey = publicKey;
        this.privateKey = privateKey;
    }

    /// <summary>
    /// Encrypt text using the RSAEncrypt class
    /// </summary>
    /// <param name="text">Text to encrypt</param>
    /// <returns>Encrypted text</returns>
    public string Encrypt(string text)
    {
        var _cryptography = new RSAEncrypt();
        byte[] bytes = Encoding.ASCII.GetBytes(text);
        var encriptBytes = _cryptography.Encrypt(bytes, publicKey);
        var base64Encript = Convert.ToBase64String(encriptBytes);
        return base64Encript;
    }

    /// <summary>
    /// Decrypt text using the RSADecrypt class
    /// </summary>
    /// <param name="text">Text to decrypt</param>
    /// <returns>Decryot text</returns>
    public string Decrypt(string base64Text)
    {
        var _cryptography = new RSADecrypt();
        byte[] bytes = Convert.FromBase64String(base64Text);
        var decriptBytes = _cryptography.Decrypt(bytes, privateKey);
        var decript = Encoding.ASCII.GetString(decriptBytes);
        return decript;
    }

    /// <summary>
    /// Encrypt text using the SHA256
    /// </summary>
    /// <param name="text">Text to encrypt</param>
    /// <returns>Encrypted text hash</returns>
    public string EncryptHash(string text)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
