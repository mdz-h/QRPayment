using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Infrastructure.MockData;
using System.IdentityModel.Tokens.Jwt;

namespace Oxxo.Cloud.Security.Infrastructure.Services
{
    public class TokenGeneratorTest
    {
        private TokenGenerator? tokenGenerator;
        private readonly string key = "ll@v3DeSegur1dad231";
        private readonly string issuer = "https://localhost:44372";
        private readonly string audience = "https://localhost:44372";

        [Theory]
        [ClassData(typeof(DeviceTokenData))]
        public void TokenGenerator_InputDeviceToken_ReturnsJwtSecurityToken(DeviceTokenDto deviceToken)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);
            /// Act
            var response = tokenGenerator.GenerateJWTToken(deviceToken);

            /// Assert    
            Assert.IsType<JwtSecurityToken>(response);
            Assert.NotNull(response);
        }

        [Theory]
        [ClassData(typeof(DeviceTokenData))]
        public void TokenGenerator_InputDeviceToken_ReturnsDateExpirationToken(DeviceTokenDto deviceToken)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);

            /// Act
            var response = tokenGenerator.GenerateJWTToken(deviceToken);

            /// Assert    
            Assert.True(response.ValidTo >= DateTime.UtcNow);
        }

        [Theory]
        [InlineData("CODE123", "DEMO", "120", "621BFBBE-0077-4A59-791E-08DAD9F60AFF")]
        public void TokenGenerator_InputDataExternalToken_ReturnsJwtSecurityToken(string? nameApplication, string? codeApplication, string timeExpirationMinutes, string externalApplicationId)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);
            /// Act
            var response = tokenGenerator.GenerateJWTToken(nameApplication, codeApplication, timeExpirationMinutes, externalApplicationId);

            /// Assert    
            Assert.IsType<JwtSecurityToken>(response);
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("CODE123", "DEMO", "120", "621BFBBE-0077-4A59-791E-08DAD9F60AFF")]
        public void TokenGenerator_InputDataExternalToken_ReturnsDateExpirationToken(string nameApplication, string codeApplication, string timeExpirationMinutes, string externalApplicationId)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);

            /// Act
            var response = tokenGenerator.GenerateJWTToken(nameApplication, codeApplication, timeExpirationMinutes, externalApplicationId);

            /// Assert    
            Assert.True(response.ValidTo >= DateTime.UtcNow);
        }

        [Fact]
        public void TokenGenerator_ReturnsRefreshTaken()
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);

            /// Act
            var response = tokenGenerator.GenerateRefreshToken();

            /// Assert    
            Assert.NotNull(response);
            Assert.NotEmpty(response);
        }

        [Theory]
        [InlineData(1, "621BFBBE-0077-4A59-791E-08DAD9F60AFF", "omar123", 120, "509YK", "10MAN","2", "Alfonso|De la cruz|Del carmen")]
        [InlineData(2, "16A0C66A-874F-483E-0C12-08DADD3463BC", "alfonso345", 20, "509YK", "10MAN", "2", "Alfonso|De la cruz|Del carmen")]
        [InlineData(3, "AC5776F0-C3DA-43B5-3792-08DAE83557A0", "fernando432", 10, "509YK", "10MAN", "2", "Alfonso|Fredel||De la cruz|Del carmen")]
        public void TokenGenerator_InputDataUserToken_ReturnsJwtSecurityToken(int personId, string operadorId, string userName, int timeExpirationMinutes, string crPlace, string crStore, string till, string fullName)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);
            /// Act
            var response = tokenGenerator.GenerateJWTToken(personId, Guid.Parse(operadorId), userName, timeExpirationMinutes, crPlace, crStore, till, fullName);

            /// Assert    
            Assert.IsType<JwtSecurityToken>(response);
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ik1BTjlZS1BST09tYXIiLCJEZXZpY2VLZXkiOiI0OGVjNTZiODNlNDM4OTk3YzZiZDYyYjY4MmYyNDZiNmI4NzU2N2IxMTE3MTY5YzAzZTlmNjlmYTVhYmU5ZDg3IiwiTnVtYmVyRGV2aWNlIjoiMiIsIklkIjoiN2RiMDhkOGItNmI5Yi1lZDExLWJmN2EtYTA0YTVlNmQ5YzdmIiwiZXhwIjoxNjc3Mzc2NzY5LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.XP8zFWbuVWsIZ_J6zCtddI_qGDhp7353m-ztmkyj_sE")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQVBQMDUiLCJzdWIiOiJBUFAwNSIsIklkIjoiNzg1ZmE5MjMtOGNhOC00Nzg1LTdjNjYtMDhkYWZkOTI4ZjQwIiwiZXhwIjoxNjc0NzQ4NzgzLCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.2kpCZRRseRFG_kkctjN6bvPv8GvkdCoLYHx2uwhDLdU")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiWHBvc0Nsb3VkT3BlcmF0b3IiLCJzdWIiOiIxNTIiLCJJZCI6IjM4MGNjMGNiLWVmZDItNGZmYi03ZTU3LTA4ZGFmZmI2Y2Q2YyIsImV4cCI6MTY3NzM4MzQ5OSwiaXNzIjoiaHR0cDovL294eG8tY2xvdWQtc2VjdXJpdHkuZGV2OjgwODAiLCJhdWQiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCJ9.uMQZ3BW0GkxxwFKfAaTBaM1T9zI4_LL7kzbyyOUYkWc")]
        public void GetNameIdClaimsToken(string token)
        {
            ///Arrange
            tokenGenerator = new TokenGenerator(key, issuer, audience);

            /// Act
            var response = tokenGenerator.GetNameIdClaimsToken(token);

            /// Assert    
            Assert.NotNull(response);
            Assert.IsType<AuthResponse>(response);
            Assert.NotNull(response.Name);
            Assert.NotEmpty(response.IdGuid);
        }
    }
}