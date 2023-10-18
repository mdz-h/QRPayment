#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class authenticate query.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Auth.Queries;
public class AuthenticateQuery : IAuthenticateQuery
{
    private readonly IApplicationDbContext context;
    private readonly ICryptographyService cryptographyService;

    /// <summary>
    /// Constructor that injects the database context and cryptography.  
    /// </summary>
    /// <param name="context">Context database</param>
    /// <param name="cryptographyService">Inject the interface with the necessary methods to encrypt and decrypt</param>
    public AuthenticateQuery(IApplicationDbContext context, ICryptographyService cryptographyService)
    {
        this.context = context;
        this.cryptographyService = cryptographyService;
    }

    /// <summary>
    /// VAlidate that the send name exists and it is active in the database, in the device table.
    /// </summary>
    /// <param name="nameDevice">The name device</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateNameDevice(string? nameDevice)
    {
        return await context.DEVICE.AsNoTracking().AnyAsync(x => x.NAME == nameDevice && x.IS_ACTIVE && x.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED);
    }

    /// <summary>
    /// First it verifies that the name device exists. After validate an active session and return its boolean value 
    /// </summary>
    /// <param name="nameDevice">The name device</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateSessionActiveDevice(string nameDevice)
    {
        var device = await context.DEVICE.AsNoTracking().Where(x => x.NAME == nameDevice && x.IS_ACTIVE && x.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED).FirstAsync();
        return await context.SESSION_TOKEN.AnyAsync(x => x.GUID == device.DEVICE_ID && x.IS_ACTIVE);
    }

    /// <summary>
    /// Validate an active session and return its boolean value 
    /// </summary>
    /// <param name="idGuid">id operator device/appExterna/user</param>
    /// <returns></returns>
    public async Task<bool> ValidateSessionActive(Guid idGuid)
    {
        return await context.SESSION_TOKEN.AnyAsync(x => x.GUID == idGuid && x.IS_ACTIVE);
    }

    /// <summary>
    /// Validates the complementary data (place,store, number device in the other) in the data base with the device name.
    /// </summary>
    /// <param name="nameDevice">The name device</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateDataDevice(string? nameDevice)
    {
        return await (from device in context.DEVICE.AsNoTracking()
                      join number in context.DEVICE_NUMBER.AsNoTracking() on device.DEVICE_NUMBER_ID equals number.DEVICE_NUMBER_ID
                      join storeplace in context.STORE_PLACE.AsNoTracking() on device.STORE_PLACE_ID equals storeplace.STORE_PLACE_ID
                      join paramCode in context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATION && x.IS_ACTIVE) on storeplace.STORE_PLACE_ID equals paramCode.STORE_PLACE_ID into grouping
                      from paramCode in grouping.DefaultIfEmpty()
                      where device.NAME == nameDevice
                      && device.IS_ACTIVE
                      && number.IS_ACTIVE
                      && storeplace.IS_ACTIVE
                      && device.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED
                      select device
                      ).AnyAsync();
    }

    /// <summary>
    /// Validate that the send code exists and it is active in the database, in the external_application table. 
    /// </summary>
    /// <param name="code">The code application external</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateCodeExternal(string code)
    {
        return await context.EXTERNAL_APPLICATION.AsNoTracking().AnyAsync(x => x.CODE == code && x.IS_ACTIVE);
    }


    /// <summary>
    /// Obtains external aplication for guid id. 
    /// </summary>
    /// <param name="guidId">The id application external</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ExternalApplication?> GetExternalApplication(Guid guidId, CancellationToken cancellationToken)
    {
        return await context.EXTERNAL_APPLICATION.AsNoTracking().Where(x => x.EXTERNAL_APPLICATION_ID == guidId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Validate that the send token exists in the database, in the session_token table.  
    /// </summary>
    /// <param name="token">The authentication token</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateToken(string token)
    {
        return await context.SESSION_TOKEN.AsNoTracking().AnyAsync(x => x.TOKEN == token);
    }

    /// <summary>
    /// Validate that the send token exists and it is active in the database, in the session_token table.   
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> ValidatePermissionToken(string? token, string endpoint)
    {
        var res = await (from st in context.SESSION_TOKEN.AsNoTracking()
                         join d in context.DEVICE.AsNoTracking()
                           on st.GUID equals d.DEVICE_ID into dn
                         from dd in dn.DefaultIfEmpty()
                         join ea in context.EXTERNAL_APPLICATION.AsNoTracking()
                           on st.GUID equals ea.EXTERNAL_APPLICATION_ID into ean
                         from ead in ean.DefaultIfEmpty()
                         join o in context.OPERATOR.AsNoTracking()
                           on st.GUID equals o.OPERATOR_ID into onn
                         from od in onn.DefaultIfEmpty()
                         join uwl in context.USER_WORKGROUP_LINK.AsNoTracking()
                           on st.GUID equals uwl.GUID into uwln
                         from uwld in uwln.DefaultIfEmpty()
                         join w in context.WORKGROUP.AsNoTracking()
                           on uwld.WORKGROUP_ID equals w.WORKGROUP_ID into wn
                         from wd in wn.DefaultIfEmpty()
                         join wpl in context.WORKGROUP_PERMISSION_LINK.AsNoTracking()
                           on wd.WORKGROUP_ID equals wpl.WORKGROUP_ID into wpln
                         from wpld in wpln.DefaultIfEmpty()
                         join p in context.PERMISSION.AsNoTracking()
                           on wpld.PERMISSION_ID equals p.PERMISSION_ID into pn
                         from pd in pn.DefaultIfEmpty()
                         join wpsl in context.WORKGROUP_PERMISSION_STORE_LINK.AsNoTracking()
                           on wpld.WORKGROUP_PERMISSION_LINK_ID equals wpsl.WORKGROUP_PERMISSION_LINK_ID into pdn
                         from wpsld in pdn.DefaultIfEmpty()
                         where (pd.CODE == endpoint
                           && st.TOKEN == token)
                         select new TokenPermission
                         {
                             IS_ACTIVE = wpld.IS_ACTIVE != null ? wpld.IS_ACTIVE : false
                         }).FirstOrDefaultAsync();

        if (res == null)
        {
            return false;
        }

        return res.IS_ACTIVE;
    }

    /// <summary>
    /// Validate that the send token exists and it is expiration is valid in the database, in the session_token table.  
    /// </summary>
    /// <param name="token">The authentication token</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateExpirationToken(string? token)
    {
        return await context.SESSION_TOKEN.AsNoTracking().AnyAsync(x => x.TOKEN == token && x.EXPIRATION_TOKEN >= DateTime.UtcNow && x.IS_ACTIVE);
    }

    /// <summary>
    /// Validate that the send refresh token exists in the database, in the session_token table.  
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public async Task<bool> ValidateExistRefreshToken(string? refreshToken)
    {
        return await context.SESSION_TOKEN.AsNoTracking().AnyAsync(x => x.REFRESH_TOKEN == refreshToken);
    }

    /// <summary>
    /// Obtains with the sending code the api key in the database, in the external_application table.  
    /// </summary>
    /// <param name="codeApplication">The code application external</param>
    /// <param name="ApiKey">Api Key value</param>
    /// <returns>The api key encript</returns>
    public async Task<string?> GetApiKeyExternalApplication(string? codeApplication, string ApiKey)
    {
        var result = await context.EXTERNAL_APPLICATION.AsNoTracking()
            .Include(i => i.APIKEYS)
            .Where(x => x.CODE == codeApplication && x.IS_ACTIVE && x.APIKEYS.Any(a => a.IS_ACTIVE && a.EXPIRATION_TIME >= DateTime.UtcNow))
            .Select(x => x.APIKEYS.Where(w => w.IS_ACTIVE).ToList()).FirstOrDefaultAsync();

        string? strApiKey = result == null ? string.Empty : result.Where(w => cryptographyService.Decrypt(w.API_KEY ?? string.Empty) == ApiKey).Select(s => s.API_KEY).FirstOrDefault();

        return strApiKey;
    }

    /// <summary>
    /// Obtains with the sending token the refresh token in the database, in the session_token table.  
    /// </summary>
    /// <param name="token">The authentication token</param>
    /// <returns>The refresh token</returns>
    public async Task<string?> GetRefreshToken(string? token)
    {
        return await context.SESSION_TOKEN.Where(x => x.TOKEN == token).Select(x => x.REFRESH_TOKEN).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Using the device id, consult the data of the place, store, etc. required to generate the token in the database.
    /// </summary>
    /// <param name="deviceId">the device identifier.</param>
    /// <returns>DeviceTokenDTO object with the information needed to generate the token.</returns>
    public async Task<DeviceTokenDto?> GetDeviceToken(Guid deviceId)
    {
        return await (from device in context.DEVICE.AsNoTracking()
                      join number in context.DEVICE_NUMBER.AsNoTracking() on device.DEVICE_NUMBER_ID equals number.DEVICE_NUMBER_ID
                      join storeplace in context.STORE_PLACE.AsNoTracking() on device.STORE_PLACE_ID equals storeplace.STORE_PLACE_ID
                      join paramCode in context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATION && x.IS_ACTIVE) on storeplace.STORE_PLACE_ID equals paramCode.STORE_PLACE_ID into grouping
                      from paramCode in grouping.DefaultIfEmpty()
                      where device.DEVICE_ID == deviceId
                      && device.IS_ACTIVE
                      && number.IS_ACTIVE
                      && storeplace.IS_ACTIVE
                      && device.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED
                      select new DeviceTokenDto
                      {
                          Key = cryptographyService.EncryptHash(string.Concat("{", storeplace.CR_PLACE, "}-{", storeplace.CR_STORE, "}-{", number.NUMBER, "}-{", device.MAC_ADDRESS, "}-{", device.IP, "}-{", device.PROCESSOR, "}-{", device.NETWORK_CARD, "}")),
                          MacAddress = device.MAC_ADDRESS,
                          IdProcessor = device.PROCESSOR,
                          IP = device.IP,
                          NetworkCard = device.NETWORK_CARD,
                          NumberDevice = number.NUMBER,
                          CrStore = storeplace.CR_STORE,
                          CrPlace = storeplace.CR_PLACE,
                          TimeTokenExpiration = paramCode == null ? context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATION && x.STORE_PLACE_ID == null).Select(x => x.PARAM_VALUE).FirstOrDefault() : paramCode.PARAM_VALUE,
                          Name = device.NAME,
                          DeviceId = device.DEVICE_ID
                      }).FirstOrDefaultAsync();

    }

    /// <summary>
    /// Using the name device, consult the data of the place, store, etc. required to generate the token in the database.
    /// </summary>
    /// <param name="nameDevice">Name device</param>
    /// <returns>DeviceTokenDTO object with the information needed to generate the token.</returns>
    public async Task<DeviceTokenDto?> GetDeviceToken(string nameDevice)
    {
        return await (from device in context.DEVICE.AsNoTracking()
                      join number in context.DEVICE_NUMBER.AsNoTracking() on device.DEVICE_NUMBER_ID equals number.DEVICE_NUMBER_ID
                      join storeplace in context.STORE_PLACE.AsNoTracking() on device.STORE_PLACE_ID equals storeplace.STORE_PLACE_ID
                      join paramCode in context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATION && x.IS_ACTIVE) on storeplace.STORE_PLACE_ID equals paramCode.STORE_PLACE_ID into grouping
                      from paramCode in grouping.DefaultIfEmpty()
                      where device.NAME == nameDevice
                      && device.IS_ACTIVE
                      && number.IS_ACTIVE
                      && storeplace.IS_ACTIVE
                      && device.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED
                      select new DeviceTokenDto
                      {
                          DeviceId = device.DEVICE_ID,
                          MacAddress = device.MAC_ADDRESS,
                          IdProcessor = device.PROCESSOR,
                          IP = device.IP,
                          NetworkCard = device.NETWORK_CARD,
                          NumberDevice = number.NUMBER,
                          CrStore = storeplace.CR_STORE,
                          CrPlace = storeplace.CR_PLACE,
                          TimeTokenExpiration = paramCode == null ? context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATION && x.STORE_PLACE_ID == null).Select(x => x.PARAM_VALUE).FirstOrDefault() : paramCode.PARAM_VALUE,
                          Name = nameDevice
                      }).FirstOrDefaultAsync();
    }

    /// <summary>
    /// First it verifies that the name device exists. After validate an active session and return its values.
    /// </summary>
    /// <param name="nameDevice">The name device</param>
    /// <returns>Boolean value</returns>
    public async Task<AuthResponse> GetSessionTokenDevice(string nameDevice)
    {
        var device = await context.DEVICE.AsNoTracking().Where(x => x.NAME == nameDevice && x.IS_ACTIVE && x.DEVICE_STATUS.CODE != GlobalConstantHelpers.DEVICESTATUSDEPRECATED).FirstAsync();
        return await context.SESSION_TOKEN.Where(x => x.GUID == device.DEVICE_ID && x.IS_ACTIVE)
            .Select(x => new AuthResponse
            {
                Auth = true,
                Token = x.TOKEN ?? string.Empty,
                RefreshToken = x.REFRESH_TOKEN ?? string.Empty,
                Exp = x.EXPIRATION_TOKEN,
            }).FirstAsync();
    }

    /// <summary>
    /// Obtains the values from session token for send guid. 
    /// </summary>
    /// <param name="guidId">The id device/appExternal/user </param>
    /// <returns>Boolean object AuthResponse with values from session token</returns>
    public async Task<AuthResponse> GetSessionToken(Guid guidId)
    {
        return await context.SESSION_TOKEN.Where(x => x.GUID == guidId && x.IS_ACTIVE)
            .Select(x => new AuthResponse
            {
                Auth = true,
                Token = x.TOKEN ?? string.Empty,
                RefreshToken = x.REFRESH_TOKEN ?? string.Empty,
                Exp = x.EXPIRATION_TOKEN,
            }).FirstAsync();
    }

    /// <summary>
    /// Validate that the send user and password exists and it is expiration is valid in the database, in the OPERATOR table.
    /// </summary>
    /// <param name="user">The operator user to validate</param>
    /// <param name="password">The password user to validate</param>
    /// <returns>Boolean value</returns>
    public async Task<bool> ValidateExistUserPassword(string user, string password)
    {
        return await context.OPERATOR.AsNoTracking()
            .Include(x => x.OPERATOR_PASSWORD)
            .AnyAsync(x => x.USER_NAME == user && x.IS_ACTIVE && x.OPERATOR_PASSWORD.Any(x => x.PASSWORD == cryptographyService.EncryptHash(password) && x.IS_ACTIVE && x.EXPIRATION_TIME >= DateTime.UtcNow));
    }

    /// <summary>
    /// Obtains the minutes expiration tokens for user.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<SystemParam?> GetTimeExpirationTokenUser(CancellationToken cancellationToken) {
        return await context.SYSTEM_PARAM.AsNoTracking().Where(x => x.PARAM_CODE == GlobalConstantHelpers.TOKENEXPIRATIONUSER && x.IS_ACTIVE).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Obtains the data for user operator.
    /// </summary>
    /// <param name="userName"><Operator user name /param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Operator> GetOperatorActiveForUserNotNull(string userName, CancellationToken cancellationToken) 
    {
        return await context.OPERATOR.AsNoTracking().Where(x => x.USER_NAME == userName && x.IS_ACTIVE).FirstAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Obtains the data for user operator with the value guid.
    /// </summary>
    /// <param name="operadorId"><Operator guid id /param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Operator?> GetOperatorActiveForGuidId(Guid operadorId, CancellationToken cancellationToken)
    {
        return await context.OPERATOR.AsNoTracking().Include(x => x.PERSON).AsNoTracking().Where(x => x.OPERATOR_ID == operadorId && x.IS_ACTIVE).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// VAlidate storce place exist
    /// </summary>
    /// <param name="crStore">CrStore param</param>
    /// <param name="crPlace">CrPlace param</param>
    /// <param name="cancellationToken">cancellation param</param>
    /// <returns></returns>
    public async Task<bool> ValidateExistAndActiveStorePlace(string crStore, string crPlace, CancellationToken cancellationToken) {
        return await context.STORE_PLACE.AsNoTracking().AnyAsync(x => x.CR_STORE == crStore && x.CR_PLACE == crPlace && x.IS_ACTIVE, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Obtains the data for user operator.
    /// </summary>
    /// <param name="userName"><Operator user name /param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Operator?> GetOperatorActiveForUser(string userName, CancellationToken cancellationToken)
    {
        return await context.OPERATOR.Include(x => x.PERSON)          
           .Where(x => x.USER_NAME == userName && x.IS_ACTIVE)
           .Select(x => x).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get a list of system parameters
    /// </summary>
    /// <returns>List of System parameters</returns>
    public async Task<List<SystemParam>> GetSystemParamsPasswordRules(CancellationToken cancellationToken)
    {
        List<string> lstSystemParams = GlobalConstantHelpers.SYSTEMPARAMSPASSWORDRULES.Split(',').ToList();
        var config = await context.SYSTEM_PARAM.Where(w => lstSystemParams.Contains((w.PARAM_CODE ?? "").ToUpper())).ToListAsync(cancellationToken);
        return config;
    }

    /// <summary>
    /// Obtains the data for user operator and password.
    /// </summary>
    /// <param name="userName"><Operator user name /param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Operator?> GetOperatorActiveForUserPassword(string userName, string password, CancellationToken cancellationToken)
    {
        var list = await context.OPERATOR
         .Include(i => i.OPERATOR_PASSWORD)
         .Where(x => x.USER_NAME == userName && x.IS_ACTIVE /*&& x.OPERATOR_PASSWORD.Any(a => a.IS_ACTIVE && a.PASSWORD == password)*/)
         .Select(x => x).ToListAsync(cancellationToken);


        return await context.OPERATOR
         .Include(i => i.OPERATOR_PASSWORD)
         .Where(x => x.USER_NAME == userName && x.IS_ACTIVE && x.OPERATOR_PASSWORD.Any(a => a.IS_ACTIVE && a.PASSWORD == password))
         .Select(x => x).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtains the data for user operator and password.
    /// </summary>
    /// <param name="userName"><Operator user name /param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OperatorStatus> GetOperadtorStatus(string codeStatus, CancellationToken cancellationToken)
    {
        return await context.OPERATOR_STATUS
         .Where(x => x.CODE == codeStatus && x.IS_ACTIVE )
         .Select(x => x).FirstAsync(cancellationToken);
    }
}