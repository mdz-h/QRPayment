using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginUser
{
    public class AuthenticateUserTest
    {
        private readonly Mock<ILogService> logService;
        private readonly ApplicationDbContext context;
        private readonly Mock<IMediator> Mediator;
        private readonly Mock<ICryptographyService> cryptographyService;
        private readonly AuthenticateQuery authenticateQuery;
        private readonly TokenGenerator tokenGenerator;
        private readonly string key = "ll@v3DeSegur1dad231";
        private readonly string issuer = "https://localhost:44372";
        private readonly string audience = "https://localhost:44372";
        private CryptographyService? cryptographySer;
        private readonly string publicKey = "<RSAKeyValue><Modulus>1Y96kiK2n0WKPY8PNHGAz714pGCS4oJEbo6x+ez/p8WIK9Xi3q4WQdJ8y5WYqBshsCmwaza4EBnYA5I+tIvL8NJ/1t2PgXilqfIFYmvfiRchnGINhIdQAKkXT/kvIq1iiNJ0E8L9F36IDqRgBXirbKcivzTFd5+gbNevp/dzMbZt61PxH99hbfEY6HlSZ0NzLDb34hekP0l4q+iMKkbPJQ2UBNtrgB2EXVBh95nTK9ZltwfGIz2Bw90PZW1a2GZFU9ulzG3onO8a8Ap3K592wqHiuGWPdQIYnOan7EZV53VVZbcYq9OaCwELV/X5x2Dk6hSdfERsAybVllLkLIl8/Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string privateKey = "<RSAKeyValue><Modulus>1Y96kiK2n0WKPY8PNHGAz714pGCS4oJEbo6x+ez/p8WIK9Xi3q4WQdJ8y5WYqBshsCmwaza4EBnYA5I+tIvL8NJ/1t2PgXilqfIFYmvfiRchnGINhIdQAKkXT/kvIq1iiNJ0E8L9F36IDqRgBXirbKcivzTFd5+gbNevp/dzMbZt61PxH99hbfEY6HlSZ0NzLDb34hekP0l4q+iMKkbPJQ2UBNtrgB2EXVBh95nTK9ZltwfGIz2Bw90PZW1a2GZFU9ulzG3onO8a8Ap3K592wqHiuGWPdQIYnOan7EZV53VVZbcYq9OaCwELV/X5x2Dk6hSdfERsAybVllLkLIl8/Q==</Modulus><Exponent>AQAB</Exponent><P>64SPw9dnX7LGVmbbLDNDzzPBg49GJeiujP07FGlfMcFDvZ8K2dl6A3XwymgGPVNVY3QgRj03qwuf+JDChuDXYqTz0WdlJMTPzzxaQIKkXJBMPSNYqzUH1Dr96FqNJNYFJ6OLhvvGxp0K3oknzdn7NtCciD6Ejk/cm2jOOz9KLec=</P><Q>6CISIN6geWELqON+jOKvaAH2bqcOtvsfeqi4X1eeWtvSF4w1u5g+z2ukiveAi4Y216Xn6MH3d0up8gwq+Se3DbSl9E0XteHVnTrL8Z39ePDEXJXnC6BVf7w8dTQ4QaxBjqc+GzJHHph1w5s92PcyWQoA23RwclGeyCTZfCtLOXs=</Q><DP>lRndMiziYvF5IyYGx1rFKR4o+rLvw/wk1VlT175F5+fkjN1tLzHBTwORp3Jn0Nc7DbVs4UrMPVsksWZj4KPjd63aNc3xeG+o6BfbQ0/x4i/wNBx2fB0ckb2vFTSOeq2Loeal5JTs6LxvLydBrc68fZ1gG9kIblgc375gIZ4Vxi0=</DP><DQ>UiDzzwNVPXyTVn6eoJP3QVIocT/T2fOBULFvAfSLB1RswX4O6L9VwacxASXOKg8jSirdoSE3P9LaXtPlRF/DySqX6JZ6BBTRsh5CV8rxCiANKUC0DQ3+EgJ0VXdTTBD45NLRC2g/d1izmbBMMn5LJut+ICbTPe8YwXEgLWlX5Q8=</DQ><InverseQ>Ig2PkF23pq41XNUWNno2ou4/lB/WFZlk8X5t1GIoxf+weTp49B2kfim3TFxuvA6pmnHGynW+GK6TPxbokaw8sxpL6RQZ0BLAPLX86rgFiQjY8ZzX7+lphPJT7qX4sgOLUUql0GxWLL2Eedth3FyzHYjvSH636Iyi3Lg8uBCcHd0=</InverseQ><D>sbFaaTGdBlUUOzhHjHlMlo2uTgdU8Ec3rU3p5GxAJFaFgQPMa0AqoMYEtFha1rUMiMmHUw4KhMscI4yRxCP1owFjAWMEvtOl9Au+UzAypJysiLRFTxpPhN0s2owZnh7qN7H0h2TctanAh35ZrvPCnvtKPBIqo+gb0bR9IaMRJDCDk9kZ3w9JEsOBy0G7jh7WZbk2/L1qp0HEvxI6J5CfXQbUs7bTmNJLown15qshEWtIna1FEukqmOqt1m4boIiTW09oqEMYPO79zvUQRZ7Xh/jc5f+ViO/N95OtpLNdfbEXDY674taYKnVYE2FoxhXcFqw7VrdQP8opbfEPoqn3OQ==</D></RSAKeyValue>";
        private readonly Mock<ISecurity> security;
        public const string Api_Key = "FlWi9BtFltfYcJu/TCwi1B5DTKbsUfOs9ECFfUWLXDm/1QysJhuabWQ0e84RuEVpwN/3uiP8yQaEVN5m1kjjxxE0zMnkuPZbo+R5+s2mMVLvsA0Dwnz1uZqMv3EG9FYwwr9zCYSYu3smK69HGaLq3EwzRbq/RGHWmygMkaO2D55gtufGqlEafn2X1bKwugoMfvtTXLpl9pH4QDJ/Jo0E3vdw4y+h+c4hud+FBsY3jqeDg5jAHHdAbbD0mgupk3Tg1OJKomI1kpFosfMV/URcDLPiBd0W/dJf08h8zIHpf0ejFZ+YQRSA2wgqjFCbUPOU+JB8J+eAciVme9MDFOdvaQ==";
        public AuthenticateUserTest()
        {
            logService = new Mock<ILogService>();
            Mediator = new Mock<IMediator>();
            cryptographyService = new Mock<ICryptographyService>();
            tokenGenerator = new TokenGenerator(key, issuer, audience);
            security = new Mock<ISecurity>();
            List<SystemParam> lstSystemParam = SetDataMock.AddSystemParam();
            List<Operator> lstOperator = SetDataMock.AddOperator();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationDbContextTEST-{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            context = new ApplicationDbContext(options, Mediator.Object, new AuditableEntitySaveChangesInterceptor()); 
            
            context.SYSTEM_PARAM.AddRange(lstSystemParam);
            context.OPERATOR.AddRange(lstOperator);
            context.OPERATOR_PASSWORD.AddRange(AddOperatorPassword());
            context.STORE_PLACE.AddRange(SetDataMock.AddStorePlaces());
            context.SaveChanges();
            authenticateQuery = new AuthenticateQuery(context, cryptographyService.Object);
        }

        [Theory]
        [InlineData("user", "morales123", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2", "CrPlace1", "CrStore1")]
        [InlineData("user", "zavala8472", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1", "CrPlace1", "CrStore1")]
        [InlineData("user", "abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2", "CrPlace1", "CrStore1")]
        public void AuthenticateUserCreateNewToken_InputDataUserPassword_ReturnAuthResponse(string typeAuth, string user, string password, string crPlace, string crStore)
        {
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrStore = crStore,
                CrPlace = crPlace
            };

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographyService.Object, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Result.Token);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.NotNull(authenticate.Result.Token);
            Assert.NotEmpty(authenticate.Result.Token);
            Assert.True(authenticate.Result.Auth);
        }

        [Theory]
        [InlineData("morales123", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2", "user" , "CrPlace1", "CrStore1")]
        [InlineData("zavala8472", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1", "user" , "CrPlace1", "CrStore1")]
        [InlineData("abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2", "user", "CrPlace1", "CrStore1")]
        public void AuthenticateUserCreateTokenExist_InputDataUserPassword_ReturnAuthResponse(string user, string password, string typeAuth, string crPlace, string crStore)
        {
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrPlace = crPlace,
                CrStore = crStore
            };
            List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographyService.Object, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Result.Token);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.NotNull(authenticate.Result.Token);
            Assert.NotEmpty(authenticate.Result.Token);
            Assert.True(authenticate.Result.Auth);
        }

        [Theory]
        [InlineData("morales123", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2", "user", "CrPlace1", "CrStore1")]
        [InlineData("zavala8472", "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1", "user", "CrPlace1", "CrStore1" )]
        [InlineData("abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2", "user","CrPlace1", "CrStore1" )]
        public void AuthenticateUserCreateTokenExistExpirated_InputDataUserPassword_ReturnAuthResponse(string user, string password, string typeAuth, string crPlace, string crStore) 
        {
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrStore = crStore,
                CrPlace = crPlace
            };
            List<SessionToken> listSessionToken = AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange            
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographyService.Object, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Result.Token);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.NotNull(authenticate.Result.Token);
            Assert.NotEmpty(authenticate.Result.Token);
            Assert.True(authenticate.Result.Auth);
        }

        [Theory]
        [InlineData("user", "morales123", "6ee59b22622b1434ee10fdd241de6d799458b913320b852641acdd5f2c453b49")]
        [InlineData("user", "zavala8472", "e89f792d97f2ac3f4ee3d31452d8ea081da627b69d1cc4b1d42884269a45f003")]
        [InlineData("user", "abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2")]
        public async Task AuthenticateUserCreateNewToken_InputDataUserPassword_ReturnException(string typeAuth, string user, string password)
        {
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
            };

            //Arrange
            var system = context.SYSTEM_PARAM.Where(x => x.PARAM_CODE == "TOKEN_EXPIRATION_USER").First();
            context.SYSTEM_PARAM.Remove(system);
            context.SaveChanges();
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographyService.Object, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Exception);
            Assert.NotNull(authenticate.Exception?.InnerException);
            CustomException exception = await Assert.ThrowsAnyAsync<CustomException>(() => authenticate);
            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData("morales123", "6ee59b22622b1434ee10fdd241de6d799458b913320b852641acdd5f2c453b49", "user")]
        [InlineData("zavala8472", "e89f792d97f2ac3f4ee3d31452d8ea081da627b69d1cc4b1d42884269a45f003", "user")]
        [InlineData("abrego52342", "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2", "user")]
        public void AuthenticateUserCreateTokenExistExpirated_InputDataUserPassword_ReturnException(string user, string password, string typeAuth)
        {
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password                
            };
            List<SessionToken> listSessionToken = AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange            
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographyService.Object, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.NotNull(authenticate.Exception);
            Assert.NotNull(authenticate.Exception?.InnerException);
            Assert.IsType<AggregateException>(authenticate.Exception);
        }


        [Theory]
        [InlineData("user_xpos", "morales123", "sPrhuoUr6yg2C1yh4Q9ChN+uEGVXWGjwSUnkTGQ3F8eMQ73+2uIyNg4WeYEtPpGB68NnhuoxL4Q2JzVRp4+GYjxaxLnymGcgvksViclc6M7dbHnydpU9Zyoslhi44suOejyZTucp0r3quYf+4vftdx47QsslFbQH7KYbDW4wtxcBX+RGlswrokb8kLg1yHKH0RtDxFOUAOhQYdEjkY8m8Lr05NhkX7e/tl+FNj9C2wMCfZT0RFyMMItFFA9P7/BEOQPW8dJoaMlr1CVi++wsRlGSFC/TeC0J6yYrHAno3f7NJCHVzuOYHXBzq+Uj1Q0ORAINAzSO02zwYQ76tDfWDA==",
        "509YK", "10MAN", "2", "20230525", "20230525", true)]
        public void AuthenticateUserXposCreateTokenExist_InputDataUserXposInternalPassword_ReturnAuthResponse(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            ///Arrange
            cryptographySer = new CryptographyService(publicKey, privateKey);

            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrPlace = crPlace,
                Till = till,
                CrStore = crStore,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };
            List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
            var authenticate = authenticateExternal.Auth(new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);           
        }

        //[Theory]
        //[InlineData("user_xpos", "morales123", "sPrhuoUr6yg2C1yh4Q9ChN+uEGVXWGjwSUnkTGQ3F8eMQ73+2uIyNg4WeYEtPpGB68NnhuoxL4Q2JzVRp4+GYjxaxLnymGcgvksViclc6M7dbHnydpU9Zyoslhi44suOejyZTucp0r3quYf+4vftdx47QsslFbQH7KYbDW4wtxcBX+RGlswrokb8kLg1yHKH0RtDxFOUAOhQYdEjkY8m8Lr05NhkX7e/tl+FNj9C2wMCfZT0RFyMMItFFA9P7/BEOQPW8dJoaMlr1CVi++wsRlGSFC/TeC0J6yYrHAno3f7NJCHVzuOYHXBzq+Uj1Q0ORAINAzSO02zwYQ76tDfWDA==",
        //"509YK", "10MAN", "2", "20230525", "20230525", false)]
        //public void AuthenticateUserXposCreateTokenExist_InputDataUserXposExternalPassword_ReturnAuthResponse(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        //{
        //    ///Arrange
        //    cryptographySer = new CryptographyService(publicKey, privateKey);
        //    context.EXTERNAL_APPLICATION.AddRange(AddExternalApplication());
        //    /// Act
        //    UserDto authDto = new()
        //    {
        //        TypeAuth = typeAuth,
        //        User = user,
        //        Password = password,
        //        Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
        //        CrPlace = crPlace,
        //        Till = till,
        //        CrStore = crStore,
        //        AdministrativeDate = administrativeDate,
        //        ProcessDate = processDate,
        //        IsInternal = isInternal
        //    };
        //    List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
        //    context.SESSION_TOKEN.AddRange(listSessionToken);
        //    context.SaveChanges();

        //    //Arrange           
        //    var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
        //    var authenticate = authenticateExternal.Auth(new CancellationToken());

        //    //Assert
        //    Assert.NotNull(authenticate);
        //    Assert.IsType<AuthResponse>(authenticate.Result);
        //    Assert.NotNull(authenticate.Result.Token);
        //}

        [Theory]
        [InlineData("user_xpos", "morales123", "1234287hdh764hvgkigh476fhfjhgugjgujgujbj484848484hfhfijdjsjksj394958vhdksjddddddddddddddddddddddjfj49494jfjf", "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public async Task AuthenticateUserXposCreateTokenExist_InputDataUserXposNotInternalPassword_ReturnAuthException(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            ///Arrange
            cryptographySer = new CryptographyService(publicKey, privateKey);
            context.EXTERNAL_APPLICATION.AddRange(AddExternalApplication());
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                CrPlace = crPlace,
                Till = till,
                CrStore = crStore,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };
            List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
            var a = authenticateExternal.Auth(new CancellationToken());
            FormatException ex = await Assert.ThrowsAsync<FormatException>(() => authenticateExternal.Auth(new CancellationToken()));
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData("user_xpos", "morales123", "1234287hdh764hvgkigh476fhfjhgugjgujgujbj484848484hfhfijdjsjksj394958vhdksjddddddddddddddddddddddjfj49494jfjf", "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public void GetAppExternaGeneraToken(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            ///Arrange
            cryptographySer = new CryptographyService(publicKey, privateKey);
            context.EXTERNAL_APPLICATION.AddRange(AddExternalApplication());
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrPlace = crPlace,
                Till = till,
                CrStore = crStore,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };
            List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
            var authenticate = authenticateExternal.GetAppExternaToken("8B32FA3D-3A48-4B91-8325-1D14F73A6398", new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.NotNull(authenticate.Result.Token);
        }

        [Theory]
        [InlineData("user_xpos", "morales123", "1234287hdh764hvgkigh476fhfjhgugjgujgujbj484848484hfhfijdjsjksj394958vhdksjddddddddddddddddddddddjfj49494jfjf", "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public void GetAppExternaTokenAppAndKeyNotExist(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            ///Arrange
            cryptographySer = new CryptographyService(publicKey, privateKey);

            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrPlace = crPlace,
                Till = till,
                CrStore = crStore,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };         

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
            var authenticate = authenticateExternal.GetAppExternaToken("8B32FA3D-3A48-4B91-8325-1D14F73A6398", new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.False(authenticate.Result.Auth);
        }


        [Theory]
        [InlineData("user_xpos", "morales123", "1234287hdh764hvgkigh476fhfjhgugjgujgujbj484848484hfhfijdjsjksj394958vhdksjddddddddddddddddddddddjfj49494jfjf", "509YK", "10MAN", "2", "20230525", "20230525", false)]
        public void GetAppExternaGeneraTokenWithoutUpdateExpirateKey(string typeAuth, string user, string password, string crStore, string crPlace, string till, string administrativeDate, string processDate, bool isInternal)
        {
            ///Arrange
            cryptographySer = new CryptographyService(publicKey, privateKey);
            var externalApplications = AddExternalApplication();
            externalApplications.First().APIKEYS.First().EXPIRATION_TIME = DateTime.UtcNow.AddDays(10);
            context.EXTERNAL_APPLICATION.AddRange(externalApplications);
            /// Act
            UserDto authDto = new()
            {
                TypeAuth = typeAuth,
                User = user,
                Password = password,
                Identification = "8B32FA3D-3A48-4B91-8325-1D14F73A6398",
                CrPlace = crPlace,
                Till = till,
                CrStore = crStore,
                AdministrativeDate = administrativeDate,
                ProcessDate = processDate,
                IsInternal = isInternal
            };
            List<SessionToken> listSessionToken = SetDataMock.AddSessionToken();
            context.SESSION_TOKEN.AddRange(listSessionToken);
            context.SaveChanges();

            //Arrange           
            var authenticateExternal = new AuthenticateUser(context, tokenGenerator, authenticateQuery, logService.Object, authDto, cryptographySer, security.Object);
            var authenticate = authenticateExternal.GetAppExternaToken("8B32FA3D-3A48-4B91-8325-1D14F73A6398", new CancellationToken());

            //Assert
            Assert.NotNull(authenticate);
            Assert.IsType<AuthResponse>(authenticate.Result);
            Assert.NotNull(authenticate.Result.Token);
        }



        public static List<SessionToken> AddSessionToken()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78";
            string refresToken = "KRy9mfvWb3VaK0pHoIyCcW74Tn/Pxw++wDsBG3R9ZPOkMRj1PmpHARmNvdUI/KCPildosn1pJcxzWP761x6ZNg==";
            List<SessionToken> lstSessionToken = new()
            {                
                new SessionToken() { GUID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"), SESSION_STATUS_ID = 3, TOKEN = token, REFRESH_TOKEN = refresToken, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(-10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SessionToken() { GUID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"), SESSION_STATUS_ID = 3, TOKEN = token, REFRESH_TOKEN = refresToken, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(-10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SessionToken() { GUID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"), SESSION_STATUS_ID = 3, TOKEN = token, REFRESH_TOKEN = refresToken, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(-10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  }
            };
            return lstSessionToken;
        }

        private static List<OperatorPassword> AddOperatorPassword()
        {
            return new List<OperatorPassword>()
            {
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e2",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"),
                    PASSWORD = "db06d81d6ebc545b8bb6c631c0bcf266f0225d1e4223b70b4edd6d6e8487f0e1",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new OperatorPassword() {
                    OPERATOR_ID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"),
                    PASSWORD = "785f17fd1f7b70c9929f4c709274eec0fef6afe676b4c27dff065b8a221692c2",
                    EXPIRATION_TIME = DateTime.UtcNow.AddDays(10),
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }

        /// <summary>
        /// Fake External Application
        /// </summary>
        public static List<ExternalApplication> AddExternalApplication()
        {
            List<ExternalApplication> lstExternalApplication = new()
            {
                 new ExternalApplication()
                 {
                    EXTERNAL_APPLICATION_ID = Guid.Parse("60252791-3EE1-4443-56E7-08DB637DF636"),
                    CODE = "INTERNAL_APP",
                    NAME = "INTERNAL_APP",
                    TIME_EXPIRATION_TOKEN = 1000,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow,
                    APIKEYS = {
                          new ApiKey() {
                                API_KEY_ID = 1,
                                EXTERNAL_APPLICATION_ID = Guid.Parse("60252791-3EE1-4443-56E7-08DB637DF636"),
                                API_KEY = Api_Key,
                                EXPIRATION_TIME = DateTime.UtcNow,
                                IS_ACTIVE= true,
                                CREATED_DATETIME= DateTime.UtcNow,
                                CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                          }
                     }
                 }
            };
            return lstExternalApplication;
        }

        public static List<ApiKey> AddApiKeys()
        {
            return new List<ApiKey>()
            {                
                new ApiKey() {
                    EXTERNAL_APPLICATION_ID = Guid.Parse("60252791-3EE1-4443-56E7-08DB637DF636"),
                    API_KEY_ID = 1,
                    API_KEY = Api_Key,
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }
    }
}
