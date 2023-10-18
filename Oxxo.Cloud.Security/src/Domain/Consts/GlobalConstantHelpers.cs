namespace Oxxo.Cloud.Security.Domain.Consts;
#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Constants helpers.
//===============================================================================
#endregion
public static class GlobalConstantHelpers
{
    public const int SESSIONSTATUSID = 3;
    public const int NUMBERDEVICEDEFAULT = 1;
    public const int MINLENGHTTOKEN = 100;
    public const int MINLENGHTREFRESHTOKEN = 50;
    public const string USERSESSIONSEQUENCE = "USER_SESSION_SEQ";
    public const string NUMBERDEVICE = "NumberDevice";
    public const string TOKENEXPIRATION = "TOKEN_EXPIRATION";
    public const string MACADRESS = "MacAdress";
    public const string TOKEN = "Token";
    public const string REFRESHTOKEN = "RefreshToken";
    public const string APIKEY = "ApiKey";
    public const string DEVICEKEY = "DeviceKey";
    public const string CODE = "CODE";
    public const string DEVICE = "device";
    public const string EXTERNAL = "external";
    public const string USER = "user";
    public const string USERXPOS = "user_xpos";
    public const string NAME = "Name";
    public const string TYPEAUTH = "TypeAuth";
    public const string PROPERTIEENDPOINT = "EndPoint";
    public const string DEVICEIDENTIFIER = "DeviceIdentifier";
    public const string DEVICENAME = "DeviceName";
    public const string CRPLACE = "CrPlace";
    public const string CRSTORE = "CrStore";
    public const string TILL = "Till";
    public const string TILLNUMBER = "TillNumber";
    public const string MACADDRESS = "MACAddress";
    public const string IP = "IP";
    public const string PROCESSOR = "Processor";
    public const string NETWORKCARD = "NetworkCard";
    public const string ISSERVER = "IsServer";
    public const string DEVICETYPETILL = "TILL";
    public const string IDGUID = "Id";
    public const string IDENTIFICATION = "identification";
    public const string USERIDENTIFICATION = "userIdentification";
    public const string METHODCREATE = "Create";
    public const string EVENTCREATEEXTERNALAPPLICATION = "CretaeExternalApplication";
    public const string NAMEEXTERNALAPPSCONTROLLER = "ExternalAppsController.cs";
    public const string METHODEXTERNALAPPCRAETEHANDLER = "CreateExternalAppsCommandHandle.Handle";
    public const string NAMECLASSEXTERNALAPPCOMMANDCREATE = "CreateExternalAppsCommand.cs";
    public const int ZERO = 0;
    public const string PARAMEXPIRATIONTIME = "PASSWORD_EXPIRATION_TIME";
    public const int NUMBEROFRECORDSFORVALIDATELASTPASSWORDS = 15;
    public const int IDOFTEMPLATEEMAILNEWADMINISTRATOR = 1;
    public const string TOKENEXPIRATIONUSER = "TOKEN_EXPIRATION_USER";
    public const int TIMEOUTEXPIRATIONTOKENMINUTS = 1440;
    public const string METHODGET = "Get";
    public const string EVENTEXTERNALAPPSGET = "Get";
    public const string EXTERNALAPPSCONTROLLER = "ExternalAppsController.cs";
    public const string EXTERNALAPPSMETHODGETHANDLER = "GetExternalAppsHandler.Handle";
    public const string NAMECLASSGETEXTERNALAPPSQUERY = "GetExternalAppsQuery.cs";
    public const string CARACTERPAID = "|";
    public const string CARACTERSPACE = " ";
    public const string FULLNAME = "FullName";
    #region NAMEPROPERTIESLOG AUTH
    public const string EVENTMETHODLOGIN = "Login";
    public const string METHODLOGIN = "login";
    public const string METHODAUTHENTICATEHANDLER = "AuthenticateHandler.Handle";
    public const string METHODAUTHENTICATEDEVICE = "AuthenticateDevice.Auth";
    public const string METHODAUTHENTICATEEXTERNAL = "AuthenticateExternal.Auth";
    public const string EVENTMETHODREFRESHTOKEN = "RefreshToken";
    public const string METHODREFRESHTOKEN = "refresh-token";
    public const string METHODREFRESHTOKENHANDLER = "TokenCommandHandler.Handle";
    public const string EVENTMETHODVALIDATETOKEN = "ValidateToken";
    public const string METHODVALIDATETOKEN = "validate-token";
    public const string METHODVALIDATETOKENHANDLER = "ValidateTokenQueryHandler.Handle";
    public const string EVENTMETHODVALIDATIONEXCEPTION = "ValidationException";
    public const string METHOGENERATELOGSERRORS = "GenerateLogsErrors";
    public const string LOG_NAME_MICROSERVICE = "Oxxo.Cloud.Security";
    public const string NAMEAUTHENTICATECONTROLLER = "AuthenticateController.cs";
    public const string NAMECLASSAUTHENTICATECOMMANDHANDLER = "AuthenticateCommand.cs";
    public const string NAMECLASSAUTHENTICATEAUTHENTICATEDEVICE = "AuthenticateDevice.cs";
    public const string NAMECLASSAUTHENTICATEAUTHENTICATEEXTERNAL = "AuthenticateExternal.cs";
    public const string NAMECLASSREFRESHTOKENCOMMANDHANDLER = "RefreshTokenCommand.cs";
    public const string NAMECLASSVALIDATETOKENQUERY = "ValidateTokenQuery.cs";
    public const string NAMECLASSVALIDATIONEXCEPTION = "ValidationException.cs";
    public const string NAMEPERMISSIONCONTROLLER = "PermissionController.cs";
    public const string NAMECLASSPERMISSIONCREATECOMMANDHANDLER = "CreatePermissionsCommand.cs";
    public const string NAMECLASSPERMISSIONUPDATECOMMANDHANDLER = "UpdatePermissionsCommand.cs";
    public const string NAMECLASSPERMISSIONDELETECOMMANDHANDLER = "DeletePermissionsCommand.cs";
    public const string NAMECLASSPERMISSIONGETPERMISSIONS = "PermissionsQuery.cs";
    public const string NAMECLASSMENUFERMODULEQUERY = "MenuPerModuleQuery.cs";

    public const string EVENTMETHODPERMISSION = "Permission";
    public const string METHODPERMISSIONCREATE = "Create";
    public const string METHODPERMISSIONUPDATE = "Update";
    public const string METHODPERMISSIONDELETE = "Delete";
    public const string METHODPERMISSIONGET = "Get";
    public const string METHODPERMISSIONGETBYID = "GetPermissionById";
    public const string METHODPERMISSIONGETBYROLEID = "GetPermissionByIdRol";
    public const string METHODASSINGPERMISSIONTOROLE = "Assignment/AssignPermissionsToRole";
    public const string METHODASSINGROLETOUSER = "Assignment/AssignRoleToUser";
    public const string METHODAUTHENTICATEUSER = "AuthenticateUser.Auth";
    public const string NAMECLASSAUTHENTICATEAUTHENTICATEUSER = "AuthenticateUser.cs";
    public const string METHODPERMISSIONGETMENUPERMODULE = "MenuPerModule";
    public const string INTERNALAPPNAME = "INTERNAL_APP";

    #endregion

    #region NAMEPROPERTIESLOG DEVICE

    public const string METHODGETDEVICES = "Get";
    public const string CODEENDPOINTDEVICES = "Devices/Get";
    public const string EVENTMETHODGETDEVICES = "Get";
    public const string NAMEDEVICECONTROLLER = "DeviceController.cs";
    public const string METHODGETDEVICESHANDLER = "GetDevicesHandler.Handle";
    public const string NAMECLASSGETDEVICESQUERY = "GetDevicesQuery.cs";

    public const string METHODREGISTER = "register";
    public const string EVENTMETHODREGISTERDEVICE = "Register";
    public const string METHOREGISTERDEVICEHANDLER = "RegisterDeviceHandler.Handle";
    public const string NAMECLASSREGISTERDEVICECOMMAND = "RegisterDeviceCommand.cs";
    public const string DEFAULTDEVICEWORKGROUPCODE = "DEVICE_TILL";

    #endregion

    #region NAMEPROPERTIESLOG ROLES
    public const string EVENTMETHODCREATEWORKGROUP = "Create";
    public const string METHODCREATEWORKGROUP = "Create";
    public const string NAMEROLESCONTROLLER = "RolesController.cs";
    public const string NAMECLASSCREATEROLECOMMANDHANDLER = "CreateRolesCommand.cs";
    public const string NAMECLASSUPDATEROLECOMMANDHANDLER = "UpdateRolesCommand.cs";
    public const string NAMECLASSDELETEROLECOMMANDHANDLER = "DeleteRolesCommand.cs";
    public const string NAMECLASSDELETEUSERROLECOMMANDHANDLER = "DeleteUserRoleCommand.cs";
    public const string NAMECLASSGETROLEQUERY = "RolesQuery.cs";

    public const string EVENTMETHODGETWORKGROUP = "Get";
    public const string METHODGETWORKGROUP = "GET";
    public const string METHODGETWORKGROUPBYID = "GetRoleById";

    public const string DEVICESTATUSOPEN = "OPEN";
    public const string DEVICESTATUSCLOSED = "CLOSED";
    public const string DEVICESTATUSDEPRECATED = "DEPRECATED";

    public const string METHODENABLED = "Enabled";

    public const string EVENTMETHODENABLEDDEVICE = "Enabled";
    public const string NAMEENABLEDDEVICECONTROLLER = "DeviceController.cs";
    public const string NAMECLASSGETROLEBYIDQUERY = "RolesByIdQuery.cs";
    public const string NAMECLASSGETROLEBYUSERQUERY = "RolesByUserIdQuery.cs";

    public const string METHODGETWORKGROUPBYUSERID = "GetRoleByUserId";
    public const string EVENTMETHODUPDATEWORKGROUP = "Update";
    public const string METHODUPDATEWORKGROUP = "Update";
    public const string EVENTMETHODDELETEWORKGROUP = "Delete";
    public const string METHODDELETEWORKGROUP = "Delete";
    public const string EVENTMETHODDELETEWORKGROUPUSER = "DeleteRoleToUser";
    public const string METHODDELETEWORKGROUPUSER = "DeleteRoleToUser";
    #endregion

    #region NAMEPROPERTIESLOG EXTERNAL APPS
    public const string METHODUPDATEEXTERNALAPPS = "Update";
    public const string EVENTMETHODUPDATEEXTERNALAPPS = "Update";
    public const string METHODDELETEEXTERNALAPPS = "Delete";
    public const string EVENTMETHODDELETEEXTERNALAPPS = "Delete";
    public const string METHODEXTERNALAPPGENERATE = "GenerateKey";
    public const string NAMECLASSUPDATEEXTERNALAPPSCOMMANDHANDLER = "UpdateExternalAppsCommand.cs";
    public const string NAMECLASSDELETEEXTERNALAPPSCOMMANDHANDLER = "DeleteExternalAppsCommand.cs";
    public const string EVENTMETHODGENERATEAPIKEYEXTERNALAPPS = "GenerateKey";

    public const string EVENTPOSTEXTERNALAPPS = "Post";
    public const string EVENTPOSTEXTERNALAPPSASSIGN = "AssignExternalAppsToRole";
    public const string METHODGENERATEAPIKEYEXTERNALAPP = "GenerateApiKeyHandler.Handle";
    public const string NAMECLASSGENERATEAPIKEYEXTERNALAPP = "GenerateApiKeyCommand.cs";
    public const string METHODASSIGNEXTERNALAPP = "AssignExternalAppsToRoleHandle.Handle";
    public const string NAMECLASSASSIGNEXTERNALAPP = "AssignExternalAppsToRoleCommand.cs";
    public const string METHODASSIGNTORLEEXTERNALAPPS = "AssignExternalAppsToRoleController";


    #endregion

    #region EnvironmentVariables
    public const string SECURITY_CONNECTION = "OXXO_CLOUD_SECURITY_CONNECTION";
    public const string JWT_KEY = "OXXO_CLOUD_SECURITY_JWT_KEY";
    public const string JWT_ISSUER = "OXXO_CLOUD_SECURITY_JWT_ISSUER";
    public const string JWT_AUDIENCE = "OXXO_CLOUD_SECURITY_JWT_AUDIENCE";
    public const string PUBLIC_KEY_CRYPTOGRAPHY = "OXXO_CLOUD_SECURITY_PUBLIC_KEY_CRYPTOGRAPHY";
    public const string PRIVATE_KEY_CRYPTOGRAPHY = "OXXO_CLOUD_SECURITY_PRIVATE_KEY_CRYPTOGRAPHY";
    public const string URL_EMAIL = "OXXO_CLOUD_SECURITY_URL_EMAIL";
    public const string URLOPERATORIDDEFAULT = "OXXO_CLOUD_SECURITY_OPERATOR_ID";
    public const string URL_PHYINVENTORY_HISTORY = "OXXO_CLOUD_PHYINVENTORY_HISTORY";
    public const string CONFIGURATION_SERVICE_LOGIN_ID_WM = "CONFIGURATION_SERVICE_LOGIN_ID_WM";
    public const string OXXO_CLOUD_EXTERNALCOMM_URL = "OXXO_CLOUD_EXTERNALCOMM_URL";
    public const string OXXO_CLOUD_TOKEN_SECURITY = "OXXO_CLOUD_TOKEN_SECURITY";
    #endregion

    #region StoredProcedures
    public const string GETSESIONTOKENACTIVO = "EXEC GET_SESION_TOKEN_ACTIVO {0}, {1};";
    #endregion


    #region [PATH CONTROLLERS]
    public const string METHOPERMISSIONHANDLERCREATE = "CreatePermissionsHandler.Handle";
    public const string METHOPERMISSIONHANDLERUPDATE = "UpdatePermissionsHandler.Handle";
    public const string METHOPERMISSIONHANDLERDELETE = "DeletePermissionsHandler.Handle";

    public const string PATHCREATEPERMISSION = "Permission/Create";
    public const string PATHUPDATEPERMISSION = "Permission/Update";
    public const string PATHDELETEPERMISSION = "Permission/Delete";
    public const string PATHGETPERMISSION = "Permission/Get";
    public const string PATHGETPERMISSIONBYID = "Permission/GetPermissionsById";
    public const string PATHGETPERMISSIONBYROLID = "Permission/GetPermissionsByIdRol";
    public const string PATHASSINGPERMISIONSTOROLE = "Permissions/Assignment/AssignPermissionsToRole";

    public const string PATH_CREATE_PERMISSION = "Permission/Create";
    public const string PATH_UPDATE_PERMISSION = "Permission/Update";


    public const string PATHCREATEROLE = "Roles/Create";
    public const string PATHUPDATEROLE = "Roles/Update";
    public const string PATHDELETEROLE = "Roles/Delete";
    public const string PATHDELETEROLETOUSER = "Roles/DeleteToUser";
    public const string PATHGETBYUSERID = "Roles/GetRoleByUserId";
    public const string PATHGETROLEBYID = "Roles/GetRoleById";
    public const string PATHGETROLE = "Roles/Get";
    public const string PATHASSINGROLETOUSER = "Roles/Assignment/AssignRoleToUser";

    public const string PATHENABLEDPERMISSION = "Devices/Enabled";

    public const string PATHUPDATEEXTERNALAPPS = "ExternalApps/Update";
    public const string PATHDELETEEXTERNALAPPS = "ExternalApps/Delete";
    #endregion


    #region [ADMINISTRATORS]
    public const string NAMEADMINISTRATORSCONTROLLER = "AdministratorsController.cs";
    public const string EVENTMETHODADMINISTRATORS = "Administrators";
    public const string METHODADMINISTRATORSDELETE = "Delete";
    public const string PATHDELETEADMINISTRATORS = "Administrators/Delete";

    public const string METHODADMINISTRATORSCREATE = "Create";
    public const string EVENTMETHODPOSTADMINISTRATORS = "Post";
    public const string CODEENDPOINTCREATEADMINISTRATOS = "Administrators/Post";
    public const string METHODCREATEADMINISTRATOSHANDLER = "CreateAdministratorHandler.Handle";
    public const string NAMECLASSCREATEADMINISTRATOSCOMMAND = "CreateAdministratorCommand.cs";

    public const string METHOADMINISTRATORHANDLER = "DeleteAdministratorsHandler.Handle";
    public const string METHOADMINISTRATORHANDLERDELETE = "DeleteAdministratorsHandler.Handle";
    public const string NAMECLASSADMINISTRATORCOMMAND = "DeleteAdministratorsCommand.cs";


    public const string METHODADMINISTRATORSUPDATE = "Update";
    public const string UPDATEENDPOINTADMINISTRATOR = "Administrators/Update";
    public const string CREATEENDPOINTADMINISTRATOR = "Administrators/Create";

    public const string METHOADMINISTRATORHANDLERUPDATE = "UpdateAdministratorsHandler.Handle";
    public const string NAMECLASSADMINISTRATORCOMMANDUPDATE = "UpdateAdministratorsCommand.cs";

    public const string METHOADMINISTRATORSHANDLERGET = "AdministratorsQueryHandler.Handle";
    public const string METHOADMINISTRATORSHANDLERGETBYID = "AdministratorsQueryByIdHandler.Handle";
    public const string PATHGETADMINISTRATORS = "Administrators/Get";
    public const string PATHGETADMINISTRATORSBYID = "Administrators/GetAdministratorById";
    public const string METHODADMINISTRATORSGET = "Get";
    public const string METHODADMINISTRATORSGETBYID = "GetAdministratorById";

    public const string ENDPOINTCHANGEPASSWORDADMINISTRATOR = "ChangePassword";
    public const string METHODADMINISTRATORSCHANGEPASSWORD = "ChangePassword";
    public const string EVENTMETHODCHANGEPASSWORD = "PostChangePassword";
    public const string METHODCHANGEPASSWORDHANDLER = "ChangePasswordHandler.Handle";
    public const string NAMECLASSCHANGEPASSWORDCOMMAND = "ChangePasswordCommand.cs";
    #endregion

    #region API_KEY
    public const string METHODEXTERNALAPPDELETE = "DeleteKey";
    public const string APIKEYSTATUS = "Activa";
    public const string EVENTMETHODDELETEAPIKEY = "DeleteKey";
    public const string METHODDELETEAPIKEY = "DeleteApiKeyHandler.Handle";
    public const string NAMECLASSDELETEPIKEY = "DeleteApiKeyCommand.cs";

    #endregion

    public const string SYSTEMPARAMSAPIKEYRULES = $"{APIKEYPREFIX},{APIKEYSECUREBYTES},{APIKEYEXPIRATIONTIME},{APIKEYEXCLUDESYMBOL}";
    public const string APIKEYPREFIX = "API_KEY_PREFIX";
    public const string APIKEYSECUREBYTES = "API_KEY_SECURE_BYTES";
    public const string APIKEYEXPIRATIONTIME = "API_KEY_EXPIRATION_TIME";
    public const string APIKEYEXCLUDESYMBOL = "API_KEY_EXCLUDE_SYMBOL";


    public const string SYSTEMPARAMSPASSWORDRULES = $"{PASSWORDEXPIRATIONTIME},{PASSWORDRULES},{PASSWORDRANDOMLETTERS},{PASSWORDMINLENGTHRULES},{PASSWORDMAXLENGTHRULES}";
    public const string PASSWORDEXPIRATIONTIME = "PASSWORD_EXPIRATION_TIME";
    public const string PASSWORDRULES = "PASSWORD_RULES";
    public const string PASSWORDRANDOMLETTERS = "PASSWORD_RANDOM_LETTERS";
    public const string PASSWORDMINLENGTHRULES = "PASSWORD_MIN_LENGTH_RULES";
    public const string PASSWORDMAXLENGTHRULES = "PASSWORD_MAX_LENGTH_RULES";

    #region Email
    public const string METHODSENDEMAIL = "SendEmail";
    public const string NAMECLASSEMAIL = "Email.cs";
    #endregion

    #region INVFIS_LOGIN

    public const string METHOD_LOGIN_INVFIS = "LoginInvFis";
    public const string LOG_METHOD_LOGIN_INVFIS = METHOD_LOGIN_INVFIS;
    public const string METHODCOMMANDHANDLER = "AuthInvfisCommand";
    public const string NAMECLASSLOGIN_INVFISINVQUERY = "AuthInvfisCommand.cs";

    public const string METHOD_PREVIUS_SUCCESS = "GetPreviousSuccess";
    public const string BEARER_WORD = "Bearer";
    #endregion

    #region PERMISSION
    public const string PERMISSIONTYPECODEFRONTEND = "FRONTEND";
    #endregion

    #region USER XML WEB METHODS
    public const string TIMEOUTUSERWEBMETHOD = "50000";
    public const string PAYLOADTYPE = "xml";
    public const string METHODGETWEBMETHOD = "get";
    public const string CONTENTTYPE = "text/xml";
    public const string KEYQUERYPARAM = "xmlin";
    public const string ENCODINGXML = "UTF-8";
    public const string VERSIONXMLUSERDECLARATION = "1.0";
    public const string NAMEXMLUSERELEMENTMAIN = "TPEDoc";
    public const string VERSIONXMLUSERATRIBUTENAME = "version";
    public const string NAMEXMLUSERELEMENTHEADER = "header";
    public const string NAMEXMLUSERELEMENTREQUEST = "request";
    public const string NAMEXMLUSERELEMENTVALIDAPASS = "validpass";

    public const string APPLICATIONXMLUSERATRIBUTENAME = "application";
    public const string ENTITYXMLUSERATRIBUTENAME = "entity";
    public const string OPERATIONXMLUSERATRIBUTENAME = "operation";
    public const string SOURCEXMLUSERATRIBUTENAME = "source";
    public const string FOLIOXMLUSERATRIBUTENAME = "folio";
    public const string PLACEXMLUSERATRIBUTENAME = "plaza";
    public const string STOREXMLUSERATRIBUTENAME = "tienda";
    public const string TILLXMLUSERATRIBUTENAME = "caja";
    public const string ADMINISTRATIVEDATEVERSIONXMLUSERATRIBUTENAME = "adDate";
    public const string PROCESSDATEVERSIONXMLUSERATRIBUTENAME = "pvDate";

    public const string APPLICATIONXMLUSERATRIBUTEVALUE = "FAC";
    public const string ENTITYXMLUSERATRIBUTEVALUE = "BANK";
    public const string OPERATIONXMLUSERATRIBUTEVALUE = "QRY09";
    public const string SOURCEXMLUSERATRIBUTEVALUE = "POS";

    public const string TYPEXMLUSERATRIBUTENAME = "type";
    public const string IDUSUARIOXMLUSERATRIBUTENAME = "idusuario";
    public const string PASSWORDXMLUSERATRIBUTENAME = "passwd";
    public const string RETRYXMLUSERATRIBUTENAME = "retry";
    public const string APPXMLUSERATRIBUTENAME = "app";
    public const string POSPKGXMLUSERATRIBUTENAME = "pospkg";

    public const string TYPEXMLUSERATRIBUTEVALUE = "venta";
    public const string RETRYXMLUSERATRIBUTEVALUE = "1";
    public const string APPXMLUSERATRIBUTEVALUE = "LOGIN";
    public const string MEDIATYPEJSON = "application/json";
    public const string HEADER_AUTHORIZATION = "Authorization";

    public const string ACTIVE_FOR_EXPIRING_OPERATORSTATUS = "AE";
    public const string ACTIVE_OPERATORSTATUS = "A";
    public const string FOLIODEFAULTRATRIBUTEVALUE = "1";
    #endregion
}