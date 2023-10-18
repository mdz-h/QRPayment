using Moq;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.InvFis.Command.AuthInvfis;
using System.Net;
using static Oxxo.Cloud.Security.Application.InvFis.Command.AuthInvfis.AuthInvfisCommand;

namespace Oxxo.Cloud.Security.Application.AuthInvFis.Commands
{
    public class AuthInvfisCommandTest
    {
        private readonly Mock<ILogService> logService;
        private readonly Mock<IExternalService> externalService;
        public AuthInvfisCommandTest()
        {
            logService = new Mock<ILogService>();
            externalService = new Mock<IExternalService>();
        }

        [Fact]
        public async Task Handle_InvalidRequest_Returns_BadRequest()
        {
            // Arrange
            var request = new AuthInvfisCommand
            {
                TokenAuth = "test-token"
            };

            var mockResponse = new GenResponse<bool>(HttpStatusCode.BadRequest, false);
            var handler = new AuthInvfisCommandHandler(logService.Object,externalService.Object);
            GenResponse<bool> response = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.Equal(mockResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Handle_With_No_Token_Request_Returns_Unauthorize()
        {
            // Arrange
            var request = new AuthInvfisCommand();
            var mockResponse = new GenResponse<bool>(HttpStatusCode.Unauthorized, false);
            var handler = new AuthInvfisCommandHandler(logService.Object, externalService.Object);
            GenResponse<bool> response = await handler.Handle(request, new CancellationToken());


            // Assert
            Assert.Equal(mockResponse.StatusCode, response.StatusCode);
            Assert.Equal(mockResponse.Body, response.Body);
        }


        [Fact]
        public async Task Handle_ValidRequest_Returns_Unauthorize_By_Unmatch_Password()
        {
            // Arrange
            var request = new AuthInvfisCommand
            {
                CrStore = "CRT",
                Auditor = 44556,
                Password = "0075894841",
                CurrentStoreDate = "04/06/2019",
                ActivatedCore = false,
                TokenAuth = "testAuth"
            };
            var mockResponse = new GenResponse<bool>(HttpStatusCode.Unauthorized, false);
            var handler = new AuthInvfisCommandHandler(logService.Object, externalService.Object);
            GenResponse<bool> response = await handler.Handle(request, new CancellationToken());


            // Asserta
            Assert.Equal(mockResponse.StatusCode, response.StatusCode);
            Assert.Equal(mockResponse.Body, response.Body);
        }

    }
}
