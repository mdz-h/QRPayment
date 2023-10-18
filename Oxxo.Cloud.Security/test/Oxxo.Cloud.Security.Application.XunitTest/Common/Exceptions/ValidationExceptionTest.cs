namespace Oxxo.Cloud.Security.Application.Common.Exceptions
{
    public class ValidationExceptionTest
    {

        [Fact]
        public void ValidationException_ReturnListStringErrors()
        {
            /// Act
            IList<string> errors = new ValidationException().Errors;

            /// Assert          
            Assert.True(errors.Count == 0);
        }
    }
}
