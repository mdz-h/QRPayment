#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class custom exception.
//===============================================================================
#endregion
using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Exceptions;
public class CustomException : Exception
{
    public readonly HttpStatusCode StatusCode;

    public CustomException()
        : base()
    {
    }
    public CustomException(string message)
    : base(message)
    {
    }
   
    public CustomException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public CustomException(string message, Exception innerException, HttpStatusCode statusCode)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
