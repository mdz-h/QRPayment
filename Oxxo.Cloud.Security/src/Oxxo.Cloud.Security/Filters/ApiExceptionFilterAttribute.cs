#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class api exception.
//===============================================================================
#endregion
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oxxo.Cloud.Security.Application.Common.Exceptions;

namespace Oxxo.Cloud.Security.WebUI.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(CustomException), HandleCustomException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }
        else
        {
            HandleGenericException(context);
            return;
        }

    }
    private static void HandleGenericException(ExceptionContext context)
    {
        context.Result = new ObjectResult(string.Empty)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }

    private void HandleCustomException(ExceptionContext context)
    {
        var exception = (CustomException)context.Exception;
        context.Result = new ObjectResult(string.Empty)
        {
            StatusCode = exception.StatusCode.GetHashCode(),
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        var error = exception.Errors.First();
        context.Result = new ObjectResult(string.Empty)
        {
            StatusCode = int.Parse(error),
        };

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;
        context.Result = new ObjectResult(string.Empty)
        {
            StatusCode = exception.StatusCode.GetHashCode(),
        };
        context.ExceptionHandled = true;
    }

}

