#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class validation exception.
//===============================================================================
#endregion
using FluentValidation.Results;
using Oxxo.Cloud.Security.Domain.Consts;

namespace Oxxo.Cloud.Security.Application.Common.Exceptions;
public class ValidationException : Exception
{
    public IList<string> Errors { get; }
    public string ErrorsMessage { get; }

    public ValidationException()
        : base(GlobalConstantMessages.LOGERRORMAINVALIDATIONS)
    {
        Errors = new List<string>();
        ErrorsMessage = string.Empty;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var keyValues = failures
           .GroupBy(e => e.PropertyName, e => e.ErrorCode)
           .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        Errors = keyValues.SelectMany(pair => pair.Value).ToList();


        var keyValuesMessage = failures
           .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
           .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        List<string> ErrorsMessage = keyValuesMessage.SelectMany(pair => pair.Value).ToList();
        this.ErrorsMessage = string.Join("", ErrorsMessage);
    }

}
