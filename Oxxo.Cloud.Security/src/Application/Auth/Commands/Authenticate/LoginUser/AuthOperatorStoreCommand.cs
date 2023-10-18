#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2023-08-11.
// Comment: Authenticate administrator users.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Auth.Commands.Refresh;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Auth.Commands.Authenticate.LoginUser
{
    public class AuthOperatorStoreCommand: IRequest<AuthResponse>
    {
        public AuthOperatorStoreCommand() 
        {          
            Operator = new();
            UserDto = new();
        }
        public UserDto UserDto { get; set; }
        public Operator Operator { get; set; }       
    }

    public class AuthOperatorStoreCommandHandler : IRequestHandler<AuthOperatorStoreCommand, AuthResponse>
    {      
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;
        private readonly IAuthenticateQuery authenticateQuery;
        private readonly ITokenGenerator tokenGenerator;

        /// <summary>
        /// Constructor that injects the interface token, database context and tokens querys.
        /// </summary>
        /// <param name="context">Context database</param>
        /// <param name="logService">log service</param>
        /// <param name="authenticateQuery">Inject the interface with the necessary methods to obtener consults in the database</param>
        /// <param name="tokenGenerator">Inject the interface with the necessary methods to generate the token</param>
        public AuthOperatorStoreCommandHandler(IApplicationDbContext context, ILogService logService, IAuthenticateQuery authenticateQuery, ITokenGenerator tokenGenerator)
        {
            this.context = context;
            this.logService = logService;
            this.authenticateQuery = authenticateQuery;
            this.tokenGenerator = tokenGenerator;
        }
       
        /// <summary>
        /// Register the operator store link and session token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<AuthResponse> Handle(AuthOperatorStoreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SystemParam systemParam = await GetSystemParameter(cancellationToken);
                StorePlace? store = await context.STORE_PLACE.FirstOrDefaultWithNoLockAsync(x => x.CR_STORE == request.UserDto.CrStore && x.CR_PLACE == request.UserDto.CrPlace && x.IS_ACTIVE, cancellationToken) ?? throw new CustomException(GlobalConstantMessages.NOTRECORD, HttpStatusCode.UnprocessableEntity);
                OperatorStoreLink? obj = await context.OPERATOR_STORE_LINK.FirstOrDefaultWithNoLockAsync(x => x.OPERATOR_ID == request.Operator.OPERATOR_ID && x.STORE_PLACE_ID == store.STORE_PLACE_ID && x.IS_ACTIVE, cancellationToken);

                if (obj == null)
                {
                    AuthResponse authResponse = await RegisterTokenAndOperator(request.Operator, systemParam.PARAM_VALUE ?? string.Empty, request.UserDto, store.STORE_PLACE_ID, cancellationToken);
                    return authResponse;
                }

                SessionToken? sessionToken = await context.SESSION_TOKEN.FirstOrDefaultWithNoLockAsync(x => x.SESSION_TOKEN_ID == obj.SESSION_TOKEN_ID && x.GUID == request.Operator.OPERATOR_ID && x.IS_ACTIVE, cancellationToken);
                if (sessionToken != null)
                {
                    AuthResponse auth = new()
                    {
                        Auth = true,
                        Token = sessionToken.TOKEN ?? string.Empty,
                        RefreshToken = sessionToken.REFRESH_TOKEN ?? string.Empty,
                        Exp = sessionToken.EXPIRATION_TOKEN,
                    };

                    if (sessionToken.EXPIRATION_TOKEN >= DateTime.UtcNow)
                    {
                        return auth;
                    }                   
                    return await UpdateToken(auth, request.UserDto.Identification, cancellationToken);
                }                
                return await RegisterTokenUpdateOperator(request.Operator, systemParam.PARAM_VALUE ?? string.Empty, request.UserDto, store.STORE_PLACE_ID, cancellationToken);
               
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREFRESHTOKEN, GlobalConstantHelpers.METHODREFRESHTOKENHANDLER, LogTypeEnum.Error, request.UserDto.UserIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORREFRESHTOKENCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSREFRESHTOKENCOMMANDHANDLER);
                throw new CustomException(ex.GetException());
            }
        }

        /// <summary>
        /// Get system parameter for user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private async Task<SystemParam> GetSystemParameter(CancellationToken cancellationToken)
        {
            SystemParam? systemParam = await authenticateQuery.GetTimeExpirationTokenUser(cancellationToken);
            if (systemParam == null || systemParam.PARAM_VALUE == null)
            {
                throw new CustomException(GlobalConstantMessages.USERTOKENTIMEEXPIRATIONNOTFOUND, HttpStatusCode.UnprocessableEntity);
            }

            return systemParam;
        }

        /// <summary>
        /// Contains all the business rules needed to create a token and register it in the database.
        /// </summary>
        /// <param name="operator">Object operator with values for create token</param>
        /// <param name="timeExpirationMinutes">Value in minutes for expiration token</param>
        /// <param name="userDto">Object user with values for create token</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AuthResponse> RegisterTokenAndOperator(Operator @operator, string timeExpirationMinutes, UserDto userDto, int storePlaceId, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwtToken = tokenGenerator.GenerateJWTToken(@operator.PERSON_ID, @operator.OPERATOR_ID, userDto.User, int.Parse(timeExpirationMinutes), userDto.CrPlace,
                userDto.CrStore, userDto.Till, userDto.FullName);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            string newRefreshToken = tokenGenerator.GenerateRefreshToken();

            OperatorStoreLink operatorStoreLink = new()
            {
                OPERATOR_ID = @operator.OPERATOR_ID,
                STORE_PLACE_ID = storePlaceId,
                IS_ACTIVE = true,
                LCOUNT = 1,
                CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                CREATED_DATETIME = DateTime.UtcNow,
                SESSIONTOKEN = new SessionToken()
                {
                    GUID = @operator.OPERATOR_ID,
                    SESSION_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
                    TOKEN = token,
                    REFRESH_TOKEN = newRefreshToken,
                    EXPIRATION_TOKEN = jwtToken.ValidTo,
                    START_DATETIME = DateTime.UtcNow,
                    END_DATETIME = DateTime.MinValue,
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                    CREATED_DATETIME = DateTime.UtcNow
                }
            };

            await context.OPERATOR_STORE_LINK.AddAsync(operatorStoreLink, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new AuthResponse
            {
                Auth = true,
                Token = token,
                RefreshToken = newRefreshToken,
                Exp = jwtToken.ValidTo
            };
        }

        /// <summary>
        /// Contains all the business rules needed to create a token and register it in the database.
        /// </summary>
        /// <param name="operator">Object operator with values for create token</param>
        /// <param name="timeExpirationMinutes">Value in minutes for expiration token</param>
        /// <param name="userDto">Object user with values for create token</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AuthResponse> RegisterTokenUpdateOperator(Operator @operator, string timeExpirationMinutes, UserDto userDto, int storePlaceId, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwtToken = tokenGenerator.GenerateJWTToken(@operator.PERSON_ID, @operator.OPERATOR_ID, userDto.User, int.Parse(timeExpirationMinutes), userDto.CrPlace,
                userDto.CrStore, userDto.Till, userDto.FullName);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            string newRefreshToken = tokenGenerator.GenerateRefreshToken();
            OperatorStoreLink operatorStoreLink = await context.OPERATOR_STORE_LINK.Where(x => x.OPERATOR_ID == @operator.OPERATOR_ID && x.STORE_PLACE_ID == storePlaceId && x.IS_ACTIVE).FirstAsync(cancellationToken);
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.database.BeginTransaction();
            try
            {                
                operatorStoreLink.LCOUNT += 1;
                operatorStoreLink.MODIFIED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification);
                operatorStoreLink.MODIFIED_DATETIME = DateTime.UtcNow;
                operatorStoreLink.SESSIONTOKEN = new SessionToken()
                {
                    GUID = @operator.OPERATOR_ID,
                    SESSION_STATUS_ID = GlobalConstantHelpers.SESSIONSTATUSID,
                    TOKEN = token,
                    REFRESH_TOKEN = newRefreshToken,
                    EXPIRATION_TOKEN = jwtToken.ValidTo,
                    START_DATETIME = DateTime.UtcNow,
                    END_DATETIME = DateTime.MinValue,
                    IS_ACTIVE = true,
                    LCOUNT = 0,
                    CREATED_BY_OPERATOR_ID = Guid.Parse(userDto.Identification),
                };
              
                context.OPERATOR_STORE_LINK.Update(operatorStoreLink);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);              
                throw;
            }           

            return new AuthResponse
            {
                Auth = true,
                Token = token,
                RefreshToken = newRefreshToken,
                Exp = jwtToken.ValidTo
            };
        }
                
        /// <summary>
        /// Gets the values sent by parameter generating an object of type tokenCommandHandler and calls the event that updates the token.
        /// </summary>
        /// <param name="auhtResponse">Contains the object with data token</param>
        /// <param name="cancellationToken"></param>
        /// <param name="source">Contains the source value for log.</param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private async Task<AuthResponse> UpdateToken(AuthResponse auhtResponse, string identification, CancellationToken cancellationToken)
        {
            try
            {
                RefreshTokenCommand command = new() { BearerToken = auhtResponse.Token, RefreshToken = auhtResponse.RefreshToken, Identification = identification };
                return await new TokenCommandHandler(context, tokenGenerator, authenticateQuery, logService).Handle(command, cancellationToken);
            }
            catch (CustomException ex)
            {
                _ = logService.Logger(GlobalConstantHelpers.EVENTMETHODLOGIN, GlobalConstantHelpers.METHODAUTHENTICATEDEVICE, LogTypeEnum.Error,
                    string.Concat(GlobalConstantMessages.LOGERRORREFRESHTOKENCOMMAND, ex.GetException()), GlobalConstantHelpers.NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE);
                throw;
            }
        }

    }
}
