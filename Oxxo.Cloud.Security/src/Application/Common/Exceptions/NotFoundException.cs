#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class notfound exception.
//===============================================================================
#endregion
using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public readonly HttpStatusCode StatusCode;
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

}
