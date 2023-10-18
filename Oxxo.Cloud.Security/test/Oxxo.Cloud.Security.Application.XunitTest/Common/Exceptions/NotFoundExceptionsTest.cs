using System.Net;

namespace Oxxo.Cloud.Security.Application.Common.Exceptions
{
    public class NotFoundExceptionsTest
    {
        [Fact]
        public void NotFoundException_ReturnsTypeNotFoundException()
        {
            /// Act
            var errors = new NotFoundException();

            /// Assert    
            Assert.NotNull(errors);
            Assert.IsType<NotFoundException>(errors);
        }

        [Theory]
        [InlineData("Token no encontrado")]
        public void NotFoundException_InputError_ReturnsMessageError(string message)
        {
            /// Act
            NotFoundException notFoundException = new NotFoundException(message);
            string error = notFoundException.Message;

            /// Assert          
            Assert.Equal(message, error);
        }

        [Theory]
        [InlineData("Token no encontrado")]
        public void NotFoundException_InputError_ReturnsMessageAndCodeError(string message)
        {
            /// Act
            NotFoundException customException = new NotFoundException(message, HttpStatusCode.NotFound);

            /// Assert          
            Assert.Equal(customException.Message, customException.Message);
            Assert.Equal(customException.StatusCode.GetHashCode(), (int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("Para ver error completo revise innerException", "Token no encontrado")]
        public void NotFoundException_InputErrors_ReturnsInnerException(string message, string messageInnerException)
        {
            /// Act  
            NotFoundException notFoundException = new NotFoundException(message, new Exception(messageInnerException));
            string error = notFoundException.Message;
            var errorInnerException = notFoundException.InnerException;

            /// Assert     
            Assert.IsType<Exception>(errorInnerException);
            Assert.NotNull(errorInnerException);
            Assert.Equal(message, error);
            Assert.Equal(messageInnerException, errorInnerException?.Message);
        }

    }
}
