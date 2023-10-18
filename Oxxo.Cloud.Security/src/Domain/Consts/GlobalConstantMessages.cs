#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Constants menssages.
//===============================================================================
#endregion
namespace Oxxo.Cloud.Security.Domain.Consts;
public static class GlobalConstantMessages
{
    public const string TYPEAUTHENTICATENOTEMPTY = "The authenticate type cannot be empty. ";
    public const string TYPEAUTHNOTFOUND = "The authenticate type does not exist. ";
    public const string DEVICENAMENOTEMPTY = "The name device cannot be empty. ";
    public const string DEVICENAMENOTFOUND = "The name device does not exist. ";
    public const string DEVICEKEYNOTEMPTY = "The device code cannot be empty. ";
    public const string DEVICEKEYNOTFOUND = "The device code does not exist. ";
    public const string VALIDATEDATADEVICE = "The sented code name does not have the place, number or store configured. ";
    public const string VALIDATEDATATOKEN = "The data token does not exist. ";
    public const string CODEEXTERNALNOTEMPTY = "The device external code cannot be empty. ";
    public const string CODEEXTERNALNOTFOUND = "The device external code does not exist. ";
    public const string APIKEYEXTERNALNOTEMPTY = "The external api key cannot be empty. ";
    public const string APIKEYEXISTACTIVE = "The api key  or application code does not exist or it is not active. ";
    public const string TOKENNOTEMPTY = "The token cannot be empty. ";
    public const string TOKENMINLENGHT = "Invalid token. ";
    public const string REFRESHTOKENEMPTY = "The refresh token cannot be empty. ";
    public const string REFRESHTOKENINVALID = "Invaid refresh token. ";
    public const string VALIDATEEXISTREFRESHTOKEN = "The refresh token does not exist. ";
    public const string VALIDATECORRESPONDSREFRESHTOKEN = "The sented refresh token does not correspond to the token. ";
    public const string ENDPOINTNOTEMPTY = "The enpoint cannot be empty. ";
    public const string JWTDEVICEAPPUSERNOTFOUND = "The token was not generated because the device/application data was not found. ";
    public const string IDENTIFICATIONOPERATORDEFUALT = "The identificator operator default cannot be empty. ";
    public const string USERNOTEMPTY = "The user cannot be empty. ";
    public const string USERPASSWORDNOTEMPTY = "The user password cannot be empty. ";
    public const string USERPASSWORDEXISTACTIVE = "The user is not active, their password is not correct, or their account has expired. ";
    public const string USERTOKENTIMEEXPIRATIONNOTFOUND = "The user is not active, the password is incorrect, or the account has expired. ";
    public const string USERADMINISTRATIVEDATE = "The user administrative date cannot be empty. ";
    public const string USERPROCESSDATE = "The user process date cannot be empty. ";
    public const string USERCRPlACEANDCRSTORENOTFOUND = "The crPlace or crStore does not exist. ";
    public const string USERWORKGROUPNOTEMPTY = "The user workgroup code cannot be empty. ";
    public const string USERSTATUSOPERATORNOTEMPTY = "The user status code cannot be empty. ";
    public const string FULLNAMEOPERATORNOTEMPTY = "The full name status cannot be empty. ";
    #region LoggerMessages

    #region AUTHENTICATE
    public const string LOGERRORAUTHENTICATEAPI = "Error in api to authenticate login - ";
    public const string LOGERRORAUTHENTICATECOMMAND = "Error in command to authenticate login - ";
    public const string LOGINITAUTHENTICATEDEVICE = "Started class to authenticate login device.";
    public const string LOGENDTAUTHENTICATEDEVICE = "Finished class to authenticate login device.";
    public const string LOGERRORAUTHENTICATEDEVICE = "Error in class to authenticate login device - ";
    public const string LOGINITAUTHENTICATEEXTERNAL = "Started class to authenticate login external.";
    public const string LOGENDTAUTHENTICATEEXTERNAL = "Finished class to authenticate login external.";
    public const string LOGERRORAUTHENTICATEEXTERNAL = "Error in class to authenticate login external - ";
    public const string LOGERRORMAINVALIDATIONS = "One or more validation errors have occurred - ";
    public const string LOGINITAUTHENTICATEUSER = "Started class to authenticate login user.";
    public const string LOGENDTAUTHENTICATEUSER = "Finished class to authenticate login user.";
    public const string LOGERRORAUTHENTICATEUSER = "Error in class to authenticate login user - ";
    public const string LOGERRORGETIDLOGINSERVICEWEBMETHOD = "An error occurred while generating the request web method. The service id was not found.";
    public const string LOGERRORGETURLLOGINSERVICEWEBMETHOD = "An error occurred while generating the request web method. The url service external comm was not found";
    public const string LOGERRORWEBMETHODCONECTION = "An error occurred in the request to Webmethod.";
    public const string LOGERRORAUTHENTICATEUSERXPOSGETAPIKEY = "Error generating internal application token, the api key is not active/valid or the application is disabled.";
    #endregion

    #region REFRESH TOKEN
    public const string LOGERRORREFRESHTOKENCOMMAND = "Error in command to ge refresh token - ";
    #endregion

    #region VALIDATE TOKEN
    public const string LOGINITVALIDATETOKENCOMMAND = "Started command to validate token.";
    public const string LOGENDVALIDATETOKENCOMMAND = "Finished command to validate token.";
    public const string LOGERRORVALIDATETOKENCOMMAND = "Error in command to validate token - ";
    public const string LOGERRORVALIDATETOKENNAMECLAIMS = "The token does not have the correct structure, it is missing the Name claims. ";
    #endregion

    #region [VALIDATE PERMISSION]
    public const string LOGERRORPERMISSIONAPICREATE = "Error in api to create permission - ";
    public const string LOGERRORPERMISSIONAPIUPDATE = "Error in api to update permission - ";
    public const string LOGERRORPERMISSIONAPIDELETE = "Error in api to delete permission - ";
    public const string LOGERRORPERMISSIONAPIGET = "Error in api to get permission - ";
    public const string PERMISSIONID = "The permission ID cannot be empty. ";
    public const string PERMISSIONCODE = "The code cannot be empty.";
    public const string PERMISSIONNAME = "The name cannot be empty. ";
    public const string PERMISSIONISACTIVENULL = "The field IsActive cannot be null.";
    public const string PERMISSIONDESCRIPTION = "The description cannot be empty.";
    public const string PERMISSIONMODULEID = "The module id cannot be empty.";
    public const string PERMISSIONTYPE = "The permission type id cannot be empty.";
    public const string PERMISSIONSKIP = "The field skip cannot be empty.";
    public const string PERMISSIONROLEID = "The field role cannot be empty.";
    public const string ASSIGMENTWORKGROUPIDNOTNULL = "The field WorkgroupId connot be empty or null.";
    public const string ASSIGMENTWORKGROUPIDNOTZERO = "The value of the Workgroup_id field must not be less than or equal to zero.";
    public const string ASSIGMENTWORKGROUPIDNOTEXIST = "The id of workgroup not record in data bases.";
    public const string ASSIGMENTGUIDNOTEXIST = "The Guid user not exist in data bases.";
    public const string ASSIGMENTWORKGROUPIDANDGUIDEXIST = "The fields workgroupId and User exist record in data bases.";
    public const string ASSIGMENTPERMISSIONIDNOTZERO = "The value of the Permission_id field must not be less than or equal to zero.";
    public const string PERMISSIONOTRECORD = "No records found in the database";
    public const string PERMISSIONHAVEROL = "The permission {0} have a role assign";
    public const string PERMISSIONISNOTACTIVE = "The permission {0} is inactive";
    public const string PERMISSIONEXISTRECORD = "One or more record exists in database.";
    public const string ASSIGMENTPERMISSIONIDNOTEXIST = "One or more Permission_Id not exist in database.";
    public const string ASSIGMENTPERMISSIONIDNOTNULL = "The field Permission_id not conteint null.";
    public const string ASSIGMENTPERMISSIONIDNOTEMPTY = "The field Permission_id cannot be empty.";
    public const string PERMISSIONEXISTS = "The permission '{0}' already exists.";
    public const string PERMISSIONTYPEEXISTS = "The permission type '{0}' not exists.";
    public const string MODULEEXISTS = "The module '{0}' not exists.";
    public const string LOGERRORGETPERMISSIONAPI = "Error in api to get permission - ";
    public const string LOGERRORASSINGPERMISSIONTOROLEAPI = "Error in api to assinged persmission to role.";

    public const string MAXLENGTHERROR = "The allowed number of characters is {0} for the {1} field.";
    public const string IDVALUEINCORRECT = "Max. the value id must be greater than 0.";
    public const string PERMISSIONTOKEN = "Not allowed permission.";
    public const string MAXVALUEFIELDMESSAGE = "Max. the value {0} must be greater than 0.";
    public const string GREATEROREQUALSTHANFIELD = "Max. the value {0} must be greater or equal than {1}.";
    public const string ERRORTOKENEXPIRED = "The token has expired.";

    public const string NOWORKGROUPRELATEDUSERID = "There is no workgroup related to the userid";
    public const string LOGERRORMENUPERMODULE = "Error in api to get menu per front end - ";
    #endregion

    #region GET DEVICE
    public const string LOGERRORGETDEVICESAPI = "Error in api to get devices - ";
    public const string LOGERRORGETDEVICESCOMMAND = "Error in command to get devices - ";
    public const string LOGNOTFOUNDGETDEVICES = "Not found devices.";
    public const string LOGVALIDATEPARAMSGETDEVICES = "Invalid parameters or request null.";
    public const string LOGINITUPDATEDEVICECOMMAND = "Started command to update the device. ";

    #region ENABLEDDEVICE   
    public const string LOGERRORENABLEDDEVICEAPI = "Error in command to validate enabled device - ";
    public const string ENABLEDDEVICEIDNOTEMPTY = "The field DeviceId is empty.";
    public const string ENABLEDDEVICEONOFFEMPTY = "Then field Enabled is empty or null.";
    public const string ENABLEDDEVICENOTFOUND = "Error Not found device.";
    #endregion
    #endregion

    #region REGISTER DEVICE
    public const string LOGERRORREGISTERDEVICEAPI = "Error in api to device register - ";
    public const string LOGENDENABLEDDEVICEAPI = "Finished api to enabled device.";
    public const string LOGINITDEPRECATEDDEVICECOMMAND = "Started command to update to deprecated the device. ";
    public const string LOGENDEGISTERDEVICECOMMAND = "Finished command to device register. ";
    public const string LOGENDUPDATEDEVICECOMMAND = "Finished command to update the device. ";
    public const string LOGENDDEPRECATEDDEVICECOMMAND = "Finished command to update to deprecated the device. ";
    public const string LOGERROREGISTERDEVICECOMMAND = "Error in command to device register - ";
    public const string CRPLACENOTEMPTY = "The CrPlace cannot be empty. ";
    public const string CRPlACENOTFOUND = "The CrPlace does not exist. ";
    public const string CRSTORENOTEMPTY = "The CrStore cannot be empty. ";
    public const string CRSTORENOTFOUND = "The CrStore does not exist. ";
    public const string TILLNUMBERNOTEMPTY = "The TillNumber cannot be zero or empty. ";
    public const string TILLNUMBERNOTFOUND = "The TillNumber does not exist. ";
    public const string MACADDRESSNOTEMPTY = "The MacAddress cannot be empty. ";
    public const string IPNOTEMPTY = "The IP cannot be empty. ";
    public const string IPNOTFOUND = "IP out of allowed range. ";
    public const string PROCESSORNOTEMPTY = "The Processor cannot be empty. ";
    public const string NETWORKCARDNOTEMPTY = "The NetworkCard cannot be empty. ";
    public const string DEVICETYPENOTFOUND = "The deviceType does not exist. ";
    public const string DEVICEIDENTIFIERNOTFOUND = "The DeviceIdentifier does not match the generated DeviceIdentifier hash. ";
    #endregion

    public const string LOG_ERROR_EXTERNALAPPSCREATE = "Error in api to externalApps - ";
    public const string LOG_ERROR_EXTERNALAPPCREATECOMMAND = "Error in command to create external app - ";
    public const string LOG_ERROR_EXTERNALAPPCREATECOMMANDEMPTY = "Error in command to create external app - already exists";
    public const string VALIDATION_CODE_EMPTY = "The code cannot be empty.";
    public const string VALIDATION_NAME_EMPTY = "The name cannot be empty. ";
    #endregion

    #region WorkgroupMessages

    public const string ROLENOTFOUND = "Role not found in database.";
    public const string WORKGROUPEMPTYID = "Role Id: not empty allowed.";
    public const string WORKGROUPEMPTYSHORTNAME = "Short Name: not empty allowed.";
    public const string WORKGROUPEMPTYCODE = "Code: not empty allowed.";
    public const string USERWORKGROUPLINKEMPTYGUID = "User Id: not empty or null allowed.";
    public const string WORKGROUPEMPTYDESCRIPTION = "Description: not empty allowed.";
    public const string WORKGROUPEMPTYGETSKIP = "Skip param must be greater than 0 and not empty.";
    public const string WORKGROUPEMPTYGETTAKE = "Take param must be greater than 0 and not empty.";
    public const string CODEALREADY = "The sented role code is already registered.";
    public const string USERWORKGROUPLINKNOTFOUND = "The user role link was not found.";
    public const string WORKGROUPNOTFOUND = "The role was not found.";
    public const string WORKGROUPIDNEGATIVE = "The role id value, must be greater than 0.";
    public const string ASSIGMENTROLETOUSERWORKGORUPGUID = "The field Guid not empty.";    
    public const string WORKGROUPEMPTYISACTIVE = "Is active: not empty allowed.";
    #region CREATE
    public const string LOGERRORWORKGROUPCREATEAPI = "Error in api create workgroup - ";
    public const string LOGERRORCREATEWORKGROUPCOMMAND = "Error in command to create workgroup - ";

    #endregion
    #region GET
    public const string LOGERRORWORKGROUPGETAPI = "Error in api get workgroup - ";
    public const string LOGERRORWORKGROUPGETBYIDAPI = "Error in api get workgroup by id- ";
    public const string LOGERRORUSERWORKGROUPLINKGETAPI = "Error in api get user workgroup link- ";
    public const string LOGERRORGETWORKGROUPQUERY = "Error in query to get workgroup - ";
    #endregion

    #region UPDATE   
    public const string LOGERRORWORKGROUPUPDATEAPI = "Error in api update workgroup - ";
    public const string LOGERRORUPDATEWORKGROUPCOMMAND = "Error in command to update workgroup - ";
    #endregion
    #endregion

    #region ExternalAppsMessages

    public const string EXTERNALAPPNOTFOUND = "The external app was not found.";
    public const string EXTERNALAPPSEMPTYID = "External application Id: not empty allowed.";
    public const string EXTERNALAPPSEMPTYISACTIVE = "Is active: not empty allowed.";
    public const string EXTERNALAPPSEMPTYNAME = "Name: not empty allowed.";
    public const string EXTERNALAPPSLENGTHNAME = "Name: Field length has been exceeded.";

    #region UPDATE
    public const string LOGERROREXTERNALAPPSUPDATEAPI = "Error in api update external apps - ";
    public const string LOGERRORUPDATEEXTERNALAPPSCOMMAND = "Error in command to update external apps - ";
    #endregion
    #region DELETE
    public const string LOGERROREXTERNALAPPSDELETEAPI = "Error in api delete external apps - ";
    public const string LOGERRORDELETEEXTERNALAPPSCOMMAND = "Error in command to delete external apps - ";
    #endregion

    #endregion

    #region GET

    #region DELETE
  
    public const string LOGERRORWORKGROUPDELETEAPI = "Error in api delete workgroup - ";
    public const string LOGERRORDELETEWORKGROUPCOMMAND = "Error in command to delete workgroup - "; 
    public const string LOGERRORUSERWORKGROUPDELETEAPI = "Error in api delete user workgroup - ";
    public const string LOGERRORDELETEUSERWORKGROUPCOMMAND = "Error in command to delete user workgroup - ";
    #endregion
    public const string LOGERRORGENERATEAPIEXTERNALAPP = "Error in api to generate API Key - ";
    #endregion

    #region [ADMINISTRATORS]

    public const string LOGERRORADMINISTRATORSAPIDELETE = "Error in api to delete administrators - ";
    public const string NORECORD = "No records found in the database.";
    public const string USERIDINVALID = "The user id is not valid";
    public const string LOGERRORADMINISTRATORSAPIUPDATE = "Error in api to update administrators - ";
    public const string NICKNAMENOTEMPTY = "The field Nickname cannot be empty. ";
    public const string USERNAMEMENOTEMPTY = "The field Username cannot be empty. ";
    public const string USERNAMEMEEXIST = "The field Username already exist. ";
    public const string NAMENOTEMPTY = "The field name cannot be empty. ";
    public const string LASTNAMEPATNOTEMPTY = "The field Lastnamepat cannot be empty. ";
    public const string EMAILNOTEMPTY = "The field Email cannot be empty. ";
    public const string EMAILINVALID = "The Email {0} is incorrect.";
    public const string NOTRECORD = "No records found in the database.";
    public const string LOGERRORGETADMINISTRATORSAPIGET = "Error in api to get administrators - ";
    public const string FIELDEMPTY = "The field {0} cannot be empty.";
    public const string FIELDNULL = "The field {0} cannot be null.";
    public const string LOGERRORCHANGEPASSWORD = "Error in api to change password for administrators - ";
    public const string LOGERRORCHANGEPASSWORDCOMMAND = "Error in command to get devices - ";

    #endregion

    #region [PERMISSIONS]
    public const string LOGERRORASSIGNPERMISSIONTOROLCOMMAND = "Error in command to assigment permission to rol command - ";
    public const string LOGERRORASSIGNTROLTOUSERCOMMAND = "Error in command to assigment to rol to user command - ";
    #endregion

    #region CREATE ADMINISTRATOR
    public const string LOGERRORCREATEADMINISTRATOR = "Error in api to create an administrator - ";
    public const string LOGERRORCREATEADMINISTRATORCOMMAND = "Error in command to create an administrator - ";
    public const string CONFIGSYSTEMPARAMPASSWORDRULES = "Not found password rules configurations.";
    public const string CONFIGSYSTEMPARAMPASSWORDRANDMOLETTER = "The system parameters [PASSWORD_RANDOM_LETTERS] is not configured.";
    public const string CONFIGSYSTEMPARAMPASSWORD = "The system parameters [PASSWORD_RULES] is not configured.";
    public const string CONFIGSYSTEMPARAMPASSWORDMIN = "The system parameters [PASSWORD_MIN_LENGTH_RULES] is not configured.";
    public const string CONFIGSYSTEMPARAMPASSWORDMAX = "The system parameters [PASSWORD_MAX_LENGTH_RULES] is not configured.";
    public const string CONFIGSYSTEMPARAMPASSWORDMINMAX = "The system parameters [PASSWORD_MAX_LENGTH_RULES] is can´t major than [PASSWORD_MIN_LENGTH_RULES]. ";
    public const string ADMINISTRATORSISFALSE = "The operator {0} is inactive.";
    public const string LOGSENDEMAILCREATEADMINISTRATORCOMMAND = "Start invoking Email class and microservice Oxxo.Cloud.Email.";
    #endregion

    #region EXTERNALAPPS
    public const string EXTERNALAPPS_ERROR_API_GET = "Error in api to get external apps- ";
    public const string EXTERNALAPPS_ERROR_GET_QUERY = "Finished command to get devices.";
    public const string EXTERNALAPPS_EMPTY = "No results were found with the indicated parameters.";
    public const string CONFIGSYSTEMPARAMAPIKEYSRULES = "Not found API Key rules configurations.";
    public const string CONFIGSYSTEMPARAMNOTFOUND = "The system parameters [{0}] is not configured.";
    public const string EXTERNALAPPS_NOTFOUND = "Not found external apps.";
    public const string EXTERNALAPPS_INVALIDAPARAMETERS = "Invalid parameters or request null.";
    public const string EXTEWRNALAPPISFALSE = "The external app {0} is inactive.";
    public const string LOGERROREXTERNALAPPTOASSIGN = "Error in api to assign external application to role - ";
    public const string NOTFOUNDEXTERNALAPP = "The external application is inactive or does not exists in the database.";
    public const string NOTFOUNDROLE = "The workgroup is inactive or does not exists in the database.";
    public const string EXTERNALAPPWASRELATIONSHIP = "The role id {0} already exist in this external application: {1}";
    public const string DEVICEDEFAULTWORKGROUPNOTFOUND = "The default workgroup was not found. ";
    #endregion

    #region [APIKEY]
    public const string LOGERRORDELETEAPIKEY = "Error in API to delete an API Key - ";  
    #endregion

    #region CHANGE PASSWORD
    public const string VALIDATEDATAUSERID = "The field UserId cannot be empty. ";
    public const string VALIDATEDATAOLDPASSWORD = "The field OldPassword cannot be empty. ";
    public const string VALIDATEDATANEWPASSWORD = "The field NewPassword cannot be empty. ";
    public const string VALIDATETYPEUSERID = "The field UserId is not valid type. ";
    public const string VALIDATEIFEXISTSADMINISTRATOR = "The UserId not exists. ";
    public const string VALIDATEPASSWORDISEQUALTOTHECURRENTPASSWORD = "The field OldPassword not is equal to the current password. ";
    public const string VALIDATENEWPASSWORD = "The field NewPassword is invalid. Fails with password rules.";
    public const string VALIDATELASTPASSWORDS = $"The field NewPassword cannot be equal that 15 last passwords.";
    public const string PASSWORDMESSAGEERORS = @"The rule [{0}] is invalid.";
    public const string PASSWORRULES = @"{0} rules were fulfilled and {1} needed";
    public const string INACTIVERECORD = "The {0} was already inactive.";
    #endregion

    #region Emial
    public const string LOGINITSENDEMAIL = "Start sending email by Oxxo.Cloud.Email microservice -";
    public const string LOGENDSENDEMAIL = "Finish sending email. Response microservice Oxxo.Cloud.Email. StatusCode:";
    public const string LOGERRORSENDEMAIL = "Sending email by microservice Oxxo.Cloud.Email returns Error:";
    #endregion

    #region BEHAVIOURS
    public const string STARTEDCOMMANDBEHAVIOURS = "Started command";
    public const string FINISHEDCOMMANDBEHAVIOURS = "Finished command";
    public const string WITHDATABEHAVIOURS = "with data";
    public const string EXECUTIONBEHAVIOURS = "execution time";
    #endregion

    #region LOGIN_INVFIS
    public const string LOGIN_INVFIS_ERROR_API_POST = "Error to complete transacction";
    public const string METHOD_PREVIUS_SUCCESS_ERROR = "Error to convert status inventory, must validate data conversion";
    
    #endregion

}

