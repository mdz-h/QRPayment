namespace Oxxo.Cloud.Security.Domain.Consts
{
    public class GlobalConstantSwagger
    {
        #region ADMINISTRATORS
        public const string TAGADMINISTATOR = "Operations about administrators.";

        public const string DELETEADMINISTATOR = "Delete an existing administrator by user Id.";
        public const string DESCDELETEADMINISTATOR = "For valid response try sending an string GUID with param name 'UserId' in a body request, anything diferent to GUID value will generate API errors.";

        public const string CREATEADMINISTATOR = "Create a user administrator by form data";
        public const string DESCCREATEADMINISTATOR = "This operation is responsible of creating an administrator by specifying all its necessary properties, once it is created it generates a password and sends it by mail. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'userName', a string with parameter name 'name', a string with parameter name 'middleName', a string with parameter name 'lastNamePat', a string with the parameter name 'lastNameMat', a string with the parameter name 'email ' in a request body, any different type of values ​​will generate API errors.";

        public const string UPDATEADMINISTATOR = "Update an existing administrator by form data.";
        public const string DESCUPDATEADMINISTATOR = "This operation is responsible for updating the values ​​of a specific administrator, in order to perform this update it is necessary to send the id by and the values ​​by the body of the request. For valid response try sending a model with a string with param name 'userId', a string with param name 'nickName', a string with param name 'userName', a string with param name 'lastNamePat', a string with param name 'lastNameMat', a string with param name 'middleName', a string with param name 'email', a boolean with param name 'isActive' in a body request, anything diferent type from values will generate API errors.";

        public const string GETADMINISTATOR = "Get the paged list of all administrator.";
        public const string DESGETADMINISTATOR = "This operation is responsible for obtaining all administrators by pagination specifying the page number and number of records. For valid response try sending the filters as query params, an integer value with param name 'pageNumber' and  integer value with param name 'itemsNumber', anything diferent type from values will generate API errors.";

        public const string GETADMINISTATORBYID = "Get existing individual administrator by id.";
        public const string DESCGETADMINISTATORBYID = "This operation is responsible for obtaining an existing individual administrator by id, it is necessary to specify the administrator id by query param. For valid response try sending the filter string GUID with param name 'UserId' as query param, anything diferent to GUID value will generate API errors.";
       
        public const string CHANGEPASSADMINISTRATOR = "Change the password for an existing administrator.";
        public const string DESCCHANGEPASSADMINISTRATOR = "This operation is responsible for changing the password of an administrator having as requirements id and old and new password. For valid response try sending an string GUID with param name 'UserId', a string  with param name 'oldPassword', a string  with param name 'newPassword' in a body request, anything diferent  type from values will generate API errors.";


        #endregion
        #region Authenticate

        public const string TAGAUTHENTICATE = "Operations about authenticate.";
        public const string AUTHENTICATELOGIN = "Generate a user session authenticating a device and app external.";
        public const string DESCAUTHENTICATELOGIN = "This operation is responsible for authenticating a device, app external and/or a user, generating a session of it if it does not exist, if it already exists, it will only return the session properties. For valid response try sending a string with param name 'id', a string with param name 'typeAuth' in a body request and in device case send a string with param name 'DEVICEKEY' as header else in external case send a string with param name 'ApiKey' as header, else in user case send a strings with user param and passord in the body the request, anything diferent type from values will generate API errors.";

        public const string AUTHENTICATEREFRESH = "Refresh the session by authenticating a device and app external";
        public const string DESCAUTHENTICATEREFRESH = "This operation is responsible to refresh the authenticating a device and app external, generating a new session for an existing authenticate. For valid response try sending an string with param name 'Authorization', a string with param name 'RefreshToken' as Headers , anything diferent type from values will generate API errors.";

        public const string AUTHENTICATEVALIDATE = "Validate session from an authenticated device and app external.";
        public const string DESCAUTHENTICATEVALIDATE = "This operation is responsible to validate the status of an authenticated device and app external. For valid response try sending an string with param name 'Authorization', anything diferent type from values will generate API errors.";

        
        #endregion

        #region DEVICE

        public const string TAGDEVICE = "Operations about device.";
        public const string REGISTERDEVICE = "Register a new device by form data.";
        public const string DESCREGISTERDEVICE = "This operation is responsible for creating new device by specifying all its necessary properties. First register, if the device already exists, update it and if the device already exists and you are sending a place or store, it changes to the one that is already in the database it is deprecated and a new record is created, if they did not change the place and store it is only updated and after all when the device is created, a role of device type is generated by default. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'deviceIdentifier', a string with parameter name 'deviceName', a string with parameter name 'crPlace', a string with parameter name 'crStore', a integer with the parameter name 'tillNumber', a string with the parameter name 'macAddress', a string with the parameter name 'ip', a string with the parameter name 'processor', a string with the parameter name 'networkCard', a boolean with the parameter name 'isServer', a string with the parameter name 'deviceType' in a request body, any different type of values ​​will generate API errors.";

        public const string GETDEVICE = "Gets a specified device when deviceIdentifier is sent else get paged list of all devices.";
        public const string DESCGETDEVICE = "This operation is responsible for obtaining a specified device when deviceIdentifier is sent else get all devices by pagination specifying the page number and number of records. For valid response try sending the filters as query params, an integer value with param name 'pageNumber', integer value with param name 'itemsNumber', and  string value with param name 'deviceIdentifier', anything diferent type from values will generate API errors.";

        public const string ENABLEDEVICE = "Enable or disable an existing device.";
        public const string DESCENABLEDEVICE = "This operation is responsible for enabling or enabling an disabling device, For valid response try sending an string with param name 'deviceId', a boolean  with param name 'enabled', in a body request, anything diferent type from values will generate API errors.";
        #endregion

        #region EXTERNALAPP
        public const string TAGEXTERNALAPPS = "Operations about external apps.";
        public const string GETEXTERNALAPPS = "Get the paged list of all external apps.";
        public const string DESCGETEXTERNALAPPS = "This operation is responsible for obtaining all devices by pagination specifying the page number and number of records, gets the api keys related to the external application and the roles related to the external application. For valid response try sending the filters as query params, an integer value with param name 'pageNumber', integer value with param name 'itemsNumber', and  string value with param name 'identifier', anything diferent type from values will generate API errors.";

        public const string CREATEEXTERNALAPPS = "Create a new external app by form data.";
        public const string DESCCREATEEXTERNALAPPS = "This operation is responsible for creating new external app by specifying all its necessary properties. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'code' and a string with parameter name 'name', in a request body, any different type of values ​​will generate API errors.";

        public const string UPDATEEXTERNALAPPS  = "Update an existing external app by form data.";
        public const string DESCUPDATEEXTERNALAPPS = "This operation is responsible for updating the values ​​of a specific external app, in order to perform this update it is necessary to send the id by and the values ​​by the body of the request. For valid response try sending a model with a string with param name 'externalAppId', a string with param name 'name', a boolean with param name 'isActive',in a body request, anything diferent type from values will generate API errors.";

        public const string DELETEEXTERNALAPPS = "Delete an existing external app by Id.";
        public const string DESCDELETEEXTERNALAPPS = "This operation is responsible for delete a specific existing external app by id. For valid response try sending an string with param name 'externalAppId' as a body params, anything diferent to the value will generate API errors.";

        public const string GENERATEAPIKEYEXTERNALAPPS = "Generate an api key for an external application.";
        public const string DESGENERATEAPIKEYEXTERNALAPPS = "This operation is responsible to generate an api key for an external application by specifying all its necessary properties. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'externalAppId' in a request body, any different type of values ​​will generate API errors.";

        public const string DELETEAPIKEYEXTERNALAPPS = "Delete an api key for an external application.";
        public const string DESDELETEAPIKEYEXTERNALAPPS = "This operation is responsible to delete an api key for an external application by specifying all its necessary properties. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'externalAppId' and a string with parameter name 'apiKey'  in a request body, any different type of values ​​will generate API errors.";

        public const string ASSINGNEXTERNALAPPS = "Assign an existing external app to available role.";
        public const string DESCASSINGNEXTERNALAPPS = "This operation is responsible for assign an external app to a role so that it has the permissions. For valid response try sending a string with param name 'rolId' a string with param name 'externalAppId' as a body params, anything diferent to the value will generate API errors.";

        #endregion

        #region PERMISSIONS
        public const string TAGPERMISSIONS = "Operations about permissions.";
        public const string CREATEPERMISSIONS = "Create a new permissions by form data.";
        public const string DESCCREATEPERMISSIONS = "This operation is responsible for creating new permission specifying all its necessary properties. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'name', a string with parameter name 'code',a string with parameter name 'name', a string with parameter name 'description', a integer with parameter name 'moduleID', a integer with parameter name 'permissionTypeID' in a request body, any different type of values ​​will generate API errors.";

        public const string UPDATEPERMISSIONS = "Update an existing permissions by form data.";
        public const string DESCUPDATEPERMISSIONS = "This operation is responsible for updating the values ​​of a specific permissions, in order to perform this update it is necessary to send the values ​​by the body of the request. For valid response try sending a model with a integer with param name 'permissionID', a string with param name 'name', a string with param name 'code', a string with param name 'description', a integer with param name 'moduleID', a integer with param name 'permissionTypeID', a boolean with param name 'isActive' in a body request, anything diferent type from values will generate API errors.";

        public const string DELETEPERMISSIONS = "Delete an existing permission by Id.";
        public const string DESCDELETEPERMISSIONS = "This operation is responsible for delete a specific existing permission by id. For valid response try sending an string with param name 'permissionID' as a body params, anything diferent to the value will generate API errors.";

        public const string GETPERMISSIONS = "Get the paged list of all permissions.";
        public const string DESCGETPERMISSIONS = "This operation is responsible for obtaining all devices by pagination specifying the page number and number of records. For valid response try sending the filters as query params, an integer value with param name 'pageNumber', integer value with param name 'itemsNumber', anything diferent type from values will generate API errors.";

        public const string GETPERMISSIONBYID = "Get existing individual permission by id.";
        public const string DESCGETPERMISSIONBYID = "This operation is responsible for obtaining an existing individual permission by id, it is necessary to specify the permission id by query param. For valid response try sending the filter integer with param name 'permissionId' as query param, anything diferent this value will generate API errors.";

        public const string GETPERMISSIONBYIDROL = "Get existing individual permission by id role.";
        public const string DESCGETPERMISSIONBYIDROL = "This operation is responsible for obtaining an existing individual permission by id role, it is necessary to specify the permission id role by query param. For valid response try sending the filter integer with param name 'roleId' as query param, anything diferent this value will generate API errors.";

        public const string ASSIGNPERMISSIONTOROLE = "Assign existing permission to available role.";
        public const string DESCASSIGNPERMISSIONTOROLE = "This operation is responsible for assign an existing individual permission with a role, it is necessary to specify the permission id and the role group. For valid response try sending a model with a integer with param name 'workgroupId' and  a integer with param name 'permissionId' in a body request, anything diferent type from values will generate API errors.";

        public const string MENUPERMODULE = "Get existing menu of Front End by role.";
        public const string DESMENYPERMODULE = "This operation is responsible for getting the corresponding menu by role to be drawn on the Front End. You need to specify the permission id role by query parameter. To get a valid response, try passing the filter integer with parameter name 'roleId' as the query parameter, any value other than this will result in API errors.";


        #endregion

        #region PERMISSIONS
        public const string TAGROLE = "Operations about roles.";

        public const string CREATEROLE = "Create a new role by form data.";
        public const string DESCCREATEROLE = "This operation is responsible for creating new role specifying all its necessary properties. In order to perform this registration, it is necessary to send the values ​​in the body of the request. For a valid response, try sending a model with a string with parameter name 'shortName', a string with parameter name 'code',a string with parameter name 'description' in a request body, any different type of values ​​will generate API errors.";

        public const string UPDATEROLE = "Update an existing role by form data.";
        public const string DESCUPDATEROLE = "This operation is responsible for updating the values ​​of a specific role, in order to perform this update it is necessary to send the the values ​​by the body of the request. For valid response try sending a model with a integer with param name 'roleId', a string with param name 'shortName', a boolean with param name 'isActive', a string with param name 'description' in a body request, anything diferent type from values will generate API errors.";

        public const string DELETEROLE = "Delete an existing role by Id.";
        public const string DESCDELETEROLE = "This operation is responsible for delete a specific existing role by id. For valid response try sending an integer with param name 'roleId' as a query params, anything diferent to the value will generate API errors.";

        public const string DELETEROLETOUSER = "Delete an existing role to user by id.";
        public const string DESCDELETEROLETOUSER = "This operation is responsible for delete a specific existing role to user by id. For valid response try sending an string with param name 'userId' as a query params, anything diferent to the value will generate API errors.";

        public const string GETROLE = "Get the paged list of all roles.";
        public const string DESCGETROLE = "This operation is responsible for obtaining all roles by pagination specifying the page number and number of records. For valid response try sending the filters as query params, an integer value with param name 'skip', integer value with param name 'take', anything diferent type from values will generate API errors.";

        public const string GETROLEBYID = "Get existing individual role by id.";
        public const string DESCGETROLEBYID = "This operation is responsible for obtaining an existing individual role by id, it is necessary to specify the role id by query param. For valid response try sending the filter integer with param name 'roleId' as query param, anything diferent this value will generate API errors.";


        public const string GETROLEBYUSERID = "Get existing individual role by user id.";
        public const string DESCGETROLEBYUSERID = "This operation is responsible for obtaining an existing individual role by user id, it is necessary to specify the user id by query param. For valid response try sending the filter string with param name 'userId' as query param, anything diferent this value will generate API errors.";

        public const string ASSIGNROLETOUSER = "Assign existing role to available user.";
        public const string DESCASSIGNROLETOUSER = "This operation is responsible for assign an existing individual permission with a role, it is necessary to specify the user id and the role group. For valid response try sending a model with a string with param name 'guid' and  a integer with param name 'workgroupId' in a body request, anything diferent type from values will generate API errors.";

        #endregion

        #region LOGIN_INFIS
        public const string AUTHENTICATELOGININVFIS = "Authentication control for the auditor user.";
        public const string DESCAUTHENTICATELOGININVFIS = "This operation is responsible for authenticating an auditor user for physical inventory microfrontend access, it is not an anonymous access, it needs a valid Jwt to be consumed";

        public const string LOGIN_INVFIS_CONTROLLER = "AuthenticateController.cs";
        #endregion
    }
}
