#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: GENERIC EXTENSION TO GET STRING EXCEPTION.
//===============================================================================
#endregion

namespace Oxxo.Cloud.Security.Application.Common.Helper
{
    /// <summary>
    /// Exception Extensions
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Get Message error, usually use to insert in the log service
        /// </summary>
        /// <param name="exception">Exception error</param>
        /// <returns>string error</returns>
        public static string GetException(this Exception exception)
        {
            string message = string.Empty;

            if (exception != null)
            {
                string innerExeption = exception.InnerException == null ? string.Empty : exception.InnerException.Message;
                string stackTrace = string.IsNullOrWhiteSpace(exception.StackTrace) ? string.Empty : exception.StackTrace;
                string exceptionMsg = exception.Message;

                message = $"Message: {exceptionMsg} | Stack Trace: {stackTrace} | Inner Exception: {innerExeption}";
            }

            return message;
        }
    }
}
