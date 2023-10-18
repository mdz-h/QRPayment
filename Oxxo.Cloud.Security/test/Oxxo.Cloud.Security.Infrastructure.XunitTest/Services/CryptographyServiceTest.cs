namespace Oxxo.Cloud.Security.Infrastructure.Services
{
    public class CryptographyServiceTest
    {
        private CryptographyService? cryptographyService;
        private readonly string publicKey = "<RSAKeyValue><Modulus>mpk2YUVZ7BkeGthu9yKeyXvYqdpdsReK8semuoE01Wt6NamOjBgshBgF96Ar5Hgm4Pb4K4b2dQl4cIUiwdT8IacjlDH86XMQzAemWzf0XMVWquJJrC5qtYy5SHlpiIqazKc2319eKlFdQu+4w/b5tj6Pra/g9h3KzfxrelpcsjosVsJk8Q9uMYSODUTMJ8sVuhFtFYMcJklZDLVz4AgiZ3pGsDmbdRne5meTaOFEcRFMXdWKJx+cnwOHUD9W8rQJ3jQAEVBBXlwVIRYrnLYR+s+N7gfDERuiEZsEy3qCmEbksfZeLYGkkCYvDLqtLnqjxAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string privateKey = "<RSAKeyValue><Modulus>mpk2YUVZ7BkeGthu9yKeyXvYqdpdsReK8semuoE01Wt6NamOjBgshBgF96Ar5Hgm4Pb4K4b2dQl4cIUiwdT8IacjlDH86XMQzAemWzf0XMVWquJJrC5qtYy5SHlpiIqazKc2319eKlFdQu+4w/b5tj6Pra/g9h3KzfxrelpcsjosVsJk8Q9uMYSODUTMJ8sVuhFtFYMcJklZDLVz4AgiZ3pGsDmbdRne5meTaOFEcRFMXdWKJx+cnwOHUD9W8rQJ3jQAEVBBXlwVIRYrnLYR+s+N7gfDERuiEZsEy3qCmEbksfZeLYGkkCYvDLqtLnqjxAE4i90uG/+YUaRvrdOWdQ==</Modulus><Exponent>AQAB</Exponent><P>xd4xhkv7KIyd4MUFS43+NTnavjuRHTTWFL5OrTlC+m5CRofCYkil6zR9qn7Ekx7VV0C3wvCxPRS9sgfFt86GW+nd6ri95BW6CHBoOB8o98MBeTFvvAdIpiGegEF1ec6U91tsWyNdDQTIL5mcCIJ+9OrMirkJlEq4Xu4kHENaXgM=</P><Q>yASwPJMxJ94l2w9bjclmMzQ+ikpBpLtbqdfEK2M1Tn5oiQlC7VkkujmW8xa6FCuiDfnJcH8Jhv5qi7wK9+gnYQK4YFYkw/rGt99XoWzRPFXntPMbxOR/LNfdHLemRTzR3v6rSb/jV5oOCuZKs6yhSo6zuMJgw175QtIOIEUibCc=</Q><DP>PWIHxRzPJq4w0Cju0piTMxnmlTtPclsQytCbAbwQ3jXUU6MMF4doCYZ4Masv+mAHWREXsN8QbN2BV7h3iuUD44GyuX1kU1y2IKYWfsqT4ADb9Sfz/MXZsMgfLrSf6BDBP9dZzxAybrZ8D+A8eptZHUu19pcetKVVewWvqCDw6Nc=</DP><DQ>ChVybRtelwv0DJ8xlmxY4qv3mzEzLgLkFCP0l9tgjJyC2KXG4gBkgZ71pTu4Fdw9R0cYiLte15dr9697PhROJ+3jLbgqEldKPWrdGB5MTiZmkBDRjZOXdcNMOm5ny4XNbtiX+hfadkgl/RavHW/Okduv49io2DCpkFzWqXnSynE=</DQ><InverseQ>YIX/FUFUlagP9QrrRbPmDYzJxY9LR/3E8O9FKkxBRN8UXIVXA6EXYf8W5lNYD6Kpfgl5Q/pel/WPeNrK84u/ZD7Swo8E2HqDWykz/N5rBEbF00xFt8WXGyPLKV214B9EZ1yJIhSFfvB3uZCudGG5EGCd6QIuYvwU3xR3P2BGtF8=</InverseQ><D>BD+bn/aw7u0Nx70U55O6+1x86sYGdxxnhB56dXuhgQ/wDtN3CGCNsJ+cYl6zdhbbMql2znNqCHNXLPhrqRn6R855CtSI172Cw6ieKLmT7Iy7wfxv+9xWsGCKovQ7MzZj5KXfuym6w7zyrCk8UZ5S5HA5sJM5wmm1Lg32wikDjR5DNHoC2iz8n36jCE8zWAjKUGEkWBKHmB7z7rivPZ8RYe+bnW7peogd0zGscb+vA+WM1Yq2y1lpHgbLsaFE1UBzEty7obJzWRFi9lHB9ZXEFjYngbj5dF8mPMp4Ju7VvuRVYJLNUr4STMZeuRQF9m58fROoKGiPfhqoYPzbbPZCrQ==</D></RSAKeyValue>";

        [Theory]
        [InlineData("CODE123")]
        public void CryptographyService_ReturnsStringEncrypt(string textToEncript)
        {
            ///Arrange
            cryptographyService = new CryptographyService(publicKey, privateKey);

            /// Act
            var response = cryptographyService.Encrypt(textToEncript);

            /// Assert    
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotEqual(textToEncript, response);
        }

        [Theory]
        [InlineData("IXCUn4k5ZgQAS4XzXV5rYTjlTLs9ROa/Q13h8AUD6w98RIWIVRNJfDWrKCfbNPnKGW39blg/AfxlsT0X5sSKae5hYuCy68IJAsxwVwxBHfHKRtiGv/yg6jP8SYSPULoFSFa6bWPYXVlOJsf5Hac18nx2UR/SIuvWp//8poN8JyJLmAy/bZT/eT/4M7Cl2UB5nwA6qyDd1BCMg3bKbNo3jDHGD8ISKvRTODHheo4E6G9CTKV08QZ5MQdP/sUahSdYZaCrgjLk+hKk3BN+bmgBcIEEoORaXVZRwhIHLEY0YX+Y33qKsbThhhgvArkakpTjvTjo/K+1JGME1SZopFwovw==")]
        public void CryptographyService_ReturnsStringDecrypt(string textToDecryt)
        {
            ///Arrange
            cryptographyService = new CryptographyService(publicKey, privateKey);

            /// Act
            var response = cryptographyService.Decrypt(textToDecryt);

            /// Assert    
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotEqual(textToDecryt, response);
        }

        [Theory]
        [InlineData("{place}-{crStore}-{number}-{macAdrres}-{ip}-{procesador}-{networkCard}")]
        public void CryptographyService_ReturnsHashEncrypt(string textToDecryt)
        {
            ///Arrange
            cryptographyService = new CryptographyService(publicKey, privateKey);

            /// Act
            var response = cryptographyService.EncryptHash(textToDecryt);

            /// Assert    
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotEqual(textToDecryt, response);
        }
    }
}
