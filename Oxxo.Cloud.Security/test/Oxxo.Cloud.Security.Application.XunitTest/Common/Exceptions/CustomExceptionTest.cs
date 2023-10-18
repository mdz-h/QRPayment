using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Exceptions
{
    public class CustomExceptionTest
    {
        [Fact]
        public void CustomException_ReturnsTypeCustomException()
        {
            /// Act
            var errors = new CustomException();

            /// Assert    
            Assert.NotNull(errors);
            Assert.IsType<CustomException>(errors);
        }

        [Theory]
        [InlineData("Token no encontrado")]
        public void CustomException_InputError_ReturnsMessageError(string message)
        {
            /// Act
            CustomException customException = new CustomException(message);

            /// Arrange
            string error = customException.Message;

            /// Assert          
            Assert.Equal(message, error);
        }

        [Theory]
        [InlineData("Token no encontrado")]
        public void CustomException_InputError_ReturnsMessageAndCodeError(string message)
        {
            /// Act
            CustomException customException = new CustomException(message, HttpStatusCode.NotFound);

            /// Assert          
            Assert.Equal(customException.Message, customException.Message);
            Assert.Equal(customException.StatusCode.GetHashCode(), (int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("Para ver error completo revise innerException", "Token no encontrado")]
        public void CustomException_InputErrors_ReturnsInnerExceptionAndCodeError(string message, string messageInnerException)
        {
            /// Act
            CustomException customException = new CustomException(message, new Exception(messageInnerException), HttpStatusCode.InternalServerError);

            /// Arrange
            string error = customException.Message;
            var errorInnerException = customException.InnerException;

            /// Assert     
            Assert.IsType<Exception>(errorInnerException);
            Assert.NotNull(errorInnerException);
            Assert.Equal(message, error);
            Assert.Equal(customException.StatusCode.GetHashCode(), (int)HttpStatusCode.InternalServerError);
            Assert.Equal(messageInnerException, errorInnerException?.Message);
        }
    }
}
