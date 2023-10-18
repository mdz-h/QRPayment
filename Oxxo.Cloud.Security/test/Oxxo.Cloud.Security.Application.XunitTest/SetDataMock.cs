#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion

using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application
{
    public static class SetDataMock
    {
        public static string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxME1BTiIsImp0aSI6IjUwOVlLIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjQ4ZWM1NmI4M2U0Mzg5OTdjNmJkNjJiNjgyZjI0NmI2Yjg3NTY3YjExMTcxNjljMDNlOWY2OWZhNWFiZTlkODciLCJOdW1iZXJEZXZpY2UiOiIyIiwiZXhwIjoxNjcxOTMxNjg4LCJpc3MiOiJodHRwOi8vb3h4by1jbG91ZC1zZWN1cml0eS5kZXY6ODA4MCIsImF1ZCI6Imh0dHA6Ly9veHhvLWNsb3VkLXNlY3VyaXR5LmRldjo4MDgwIn0.-3ZBASxR2NMOIw_uxwBCGwrOL3Rh0gVau9to-zBcv78";
        public static string EndPoint = string.Empty;
        private static string EndPointDefault = "Example";
        public static Guid GuidSessionToken = Guid.NewGuid();
        public const string Api_Key = "FlWi9BtFltfYcJu/TCwi1B5DTKbsUfOs9ECFfUWLXDm/1QysJhuabWQ0e84RuEVpwN/3uiP8yQaEVN5m1kjjxxE0zMnkuPZbo+R5+s2mMVLvsA0Dwnz1uZqMv3EG9FYwwr9zCYSYu3smK69HGaLq3EwzRbq/RGHWmygMkaO2D55gtufGqlEafn2X1bKwugoMfvtTXLpl9pH4QDJ/Jo0E3vdw4y+h+c4hud+FBsY3jqeDg5jAHHdAbbD0mgupk3Tg1OJKomI1kpFosfMV/URcDLPiBd0W/dJf08h8zIHpf0ejFZ+YQRSA2wgqjFCbUPOU+JB8J+eAciVme9MDFOdvaQ==";
        public const string Api_Key_Decrypt = "OXXOCLOUDPRIXoaafa0iyEMEeeKUVlco1oz8YJyCGiplwqOZdhXVD6WuZKLFsB1kx1HIr1frkf9yg6wcBPjhkoSb2Brpoo6T5F1P7QfbMw1UKTFv3eYTH9oUg5YRynXz";
        #region [DATA]


        /// <summary>
        /// Fake System Parameters
        /// </summary>
        public static List<SystemParam> AddSystemParam()
        {
            List<SystemParam> lstSystemParam = new()
            {
                new SystemParam() { SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "PASSWORD_RULES" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = "{\"LimitExpressionOK\":5,\"Expressions\":[\"[0-9]\\u002B\",\"[A-Z]\\u002B\",\"^.{8,15}$\",\"[a-z]\\u002B\",\"[!@#$%^\\u0026*()_\\u002B=\\\\[{\\\\]};:\\u003C\\u003E|./?,-]\"]}"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },

                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "PASSWORD_EXPIRATION_TIME" , CODE_PARAM_VALUE_TYPE = "INTEGER_PARAM_TYPE", PARAM_VALUE = "60"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "PASSWORD_RANDOM_LETTERS" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_+=\\[{\\]};:<>|./?,-"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "PASSWORD_MIN_LENGTH_RULES" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = "8"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "PASSWORD_MAX_LENGTH_RULES" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = "15"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },

                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "API_KEY_PREFIX" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = "OXXOCLOUDPR"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "API_KEY_SECURE_BYTES" , CODE_PARAM_VALUE_TYPE = "INT_PARAM_TYPE", PARAM_VALUE = "128"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "API_KEY_EXPIRATION_TIME" , CODE_PARAM_VALUE_TYPE = "INT_PARAM_TYPE", PARAM_VALUE = "60"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = 1, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "1", PARAM_CODE = "API_KEY_EXCLUDE_SYMBOL" , CODE_PARAM_VALUE_TYPE = "STRING_PARAM_TYPE", PARAM_VALUE = @"+,=,\,/"  , IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },

                new SystemParam(){SYSTEM_PARAM_ID_SOURCE = 2, SYSTEM_PARAM_VALUE_ID_SOURCE = null, STORE_PLACE_ID = null, CR_STORE = null, CR_PLACE = null, CODE_PARAM_TYPE = "2", PARAM_CODE = "TOKEN_EXPIRATION_USER" , CODE_PARAM_VALUE_TYPE = "INTEGER_PARAM_TYPE", PARAM_VALUE = "43800"  , IS_ACTIVE = true, LCOUNT = 0, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },

            };
            return lstSystemParam;
        }

        /// <summary>
        /// Fake Session Token 
        /// </summary>
        public static List<SessionToken> AddSessionToken()
        {
            List<SessionToken> lstSessionToken = new()
            {
                new SessionToken() { GUID = GuidSessionToken, SESSION_STATUS_ID = 3, TOKEN = Token, REFRESH_TOKEN = Token, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SessionToken() { GUID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"), SESSION_STATUS_ID = 3, TOKEN = Token, REFRESH_TOKEN = Token, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SessionToken() { GUID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"), SESSION_STATUS_ID = 3, TOKEN = Token, REFRESH_TOKEN = Token, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
                new SessionToken() { GUID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"), SESSION_STATUS_ID = 3, TOKEN = Token, REFRESH_TOKEN = Token, EXPIRATION_TOKEN = DateTime.UtcNow.AddDays(10), START_DATETIME = DateTime.UtcNow, END_DATETIME = DateTime.MinValue, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  }
            };
            return lstSessionToken;
        }

        /// <summary>
        /// Fake device
        /// </summary>
        public static List<Domain.Entities.Device> AddDevice()
        {
            List<Domain.Entities.Device> lstDevice = new()
            {
                new Domain.Entities.Device()
                {
                    DEVICE_ID = GuidSessionToken,
                    STORE_PLACE_ID = 1,
                    DEVICE_TYPE_ID = 1,
                    DEVICE_STATUS_ID = 1,
                    DEVICE_NUMBER_ID = 1,
                    MAC_ADDRESS = "55:4A:2B:37:F3:56",
                    IP= "10.106.11.226",
                    PROCESSOR = "BFEBFBFF00031777",
                    NETWORK_CARD = "3217EE16-2B38-4B5F-904F-2CE29431F21A",
                    NAME = "MAN9YKOMAR",
                    DESCRIPTION = "MAN9YKOMAR",
                    IS_SERVER = true,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                }
            };
            return lstDevice;
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
                    EXTERNAL_APPLICATION_ID = GuidSessionToken,
                    CODE = "RP",
                    NAME = "UT Name",
                    TIME_EXPIRATION_TOKEN = 0,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                }
            };
            return lstExternalApplication;
        }

        /// <summary>
        /// Fake operators 
        /// </summary>
        /// <returns>List Operators</returns>
        public static List<Operator> AddOperator()
        {
            List<Operator> lstOperator = new()
            {
                new Operator() { OPERATOR_ID = GuidSessionToken, PERSON_ID = 1, OPERATOR_STATUS_ID = 1, USER_NAME = "Fredel", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.NewGuid(), PERSON_ID = 2, OPERATOR_STATUS_ID = 1, USER_NAME = "Luis", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.NewGuid(), PERSON_ID = 3, OPERATOR_STATUS_ID = 1, USER_NAME = "Francisco", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.NewGuid(), PERSON_ID = 4, OPERATOR_STATUS_ID = 1, USER_NAME = "Fernando", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.Parse("63df3ad9-0a5f-490b-bd4f-424b3bbd5c49"), PERSON_ID = 5, OPERATOR_STATUS_ID = 1, USER_NAME = "morales123", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.Parse("33df3ad9-0a5f-490b-bd4f-424b3bbd5c11"), PERSON_ID = 6, OPERATOR_STATUS_ID = 1, USER_NAME = "zavala8472", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null },
                new Operator() { OPERATOR_ID = Guid.Parse("380CC0CB-EFD2-4FFB-7E57-08DAFFB6CD6C"), PERSON_ID = 7, OPERATOR_STATUS_ID = 1, USER_NAME = "abrego52342", FL_INTRN = true, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = null, MODIFIED_DATETIME = null }
            };
            return lstOperator;
        }

        /// <summary>
        /// Fake Workgroup  
        /// </summary>
        public static List<Workgroup> AddWorkgroup()
        {
            List<Workgroup> lstWorkgroup = new List<Workgroup>();
            lstWorkgroup.Add(
                new Workgroup() { WORKGROUP_ID = 1, CODE = "CODe", NAME = "Example", DESCRIPTION = "OXXO", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow }
            );
            lstWorkgroup.Add(
                new Workgroup() { WORKGROUP_ID = 2, CODE = "CODe", NAME = "Example", DESCRIPTION = "OXXO", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow }
            );
            return lstWorkgroup;
        }

        /// <summary>
        /// Fake Workgroup permission link 
        /// </summary>
        public static List<WorkgroupPermissionLink> AddWorkgroupPermissionLink()
        {
            List<WorkgroupPermissionLink> lstWorkgroupPermissionLink = new()
            {
                new WorkgroupPermissionLink(){ WORKGROUP_ID = 1, PERMISSION_ID = 1,  IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, MODIFIED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")  }

            };
            return lstWorkgroupPermissionLink;
        }

        /// <summary>
        /// Fake Permission
        /// </summary>
        public static List<Permission> AddPermissions()
        {
            List<Permission> lstPermission = new List<Permission>();

            lstPermission.Add(
                new Permission()
                {
                    PERMISSION_ID = 1,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 1,
                    CODE = string.IsNullOrWhiteSpace(EndPoint) ? EndPointDefault : EndPoint,
                    NAME = "Example",
                    DESCRIPTION = "OXXO",
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                });
            lstPermission.Add(
            new Permission()
            {
                PERMISSION_ID = 2,
                MODULE_ID = 1,
                PERMISSION_TYPE_ID = 1,
                CODE = string.IsNullOrWhiteSpace(EndPoint) ? EndPointDefault : EndPoint,
                NAME = "Example",
                DESCRIPTION = "OXXO",
                IS_ACTIVE = true,
                LCOUNT = 1,
                CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                CREATED_DATETIME = DateTime.UtcNow
            });
            lstPermission.Add(
                new Permission()
                {
                    PERMISSION_ID = 3,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 1,
                    CODE = string.IsNullOrWhiteSpace(EndPoint) ? EndPointDefault : EndPoint,
                    NAME = "Example",
                    DESCRIPTION = "OXXO",
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                });
            lstPermission.Add(
                new Permission()
                {
                    PERMISSION_ID = 4,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 1,
                    CODE = string.IsNullOrWhiteSpace(EndPoint) ? EndPointDefault : EndPoint,
                    NAME = "Example",
                    DESCRIPTION = "OXXO",
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                });
            lstPermission.Add(
                new Permission()
                {
                    PERMISSION_ID = 5,
                    MODULE_ID = 1,
                    PERMISSION_TYPE_ID = 1,
                    CODE = string.IsNullOrWhiteSpace(EndPoint) ? EndPointDefault : EndPoint,
                    NAME = "Example",
                    DESCRIPTION = "OXXO",
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                });


            return lstPermission;
        }


        /// <summary>
        /// Fake User Workgroup Link  
        /// </summary>
        public static List<UserWorkgroupLink> AddUserWorkgroupLink()
        {
            List<UserWorkgroupLink> UserWorkgroupLink = new()
            {
                new UserWorkgroupLink()
                {
                    GUID = GuidSessionToken,
                    WORKGROUP_ID = 1,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                },
                new UserWorkgroupLink()
                {
                    GUID = new Guid("A283FBB9-5F7E-4D64-E3E6-08DADD1D9E4E"),
                    WORKGROUP_ID = 2,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                },
                new UserWorkgroupLink()
                {
                    GUID = new Guid("16A0C66A-874F-483E-0C12-08DADD3463BC"),
                    WORKGROUP_ID = 1,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow
                }

            };
            return UserWorkgroupLink;
        }

        /// <summary>
        /// Fake Workgroup Permission Store Link 
        /// </summary>
        public static List<WorkgroupPermissionStoreLink> AddWorkgroupPermissionStoreLink()
        {
            List<WorkgroupPermissionStoreLink> lstWorkgroupPermissionStoreLink = new()
            {
                new WorkgroupPermissionStoreLink()
                {
                    WORKGROUP_PERMISSION_STORE_LINK_ID = 1,
                    WORKGROUP_PERMISSION_LINK_ID = 1,
                    STORE_PLACE_ID = 1,
                    IS_ACTIVE = true,
                    LCOUNT = 1,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"),
                    CREATED_DATETIME = DateTime.UtcNow,
                    MODIFIED_BY_OPERATOR_ID= Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
            return lstWorkgroupPermissionStoreLink;
        }

        /// <summary>
        /// Fake Permission Module
        /// </summary>
        public static List<Module> AddModule()
        {
            List<Module> lstModule = new()
            {
                new Module(){ APPLICATION_ID = 1, NAME="ENDPOINT", DESCRIPTION = "ENDPOINT", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  }
            };
            return lstModule;
        }

        /// <summary>
        /// Fake Permission Type
        /// </summary>
        public static List<PermissionType> AddPermissionsType()
        {
            List<PermissionType> lstPermissionType = new()
            {
                new PermissionType(){ CODE = "ENDPOINT", NAME="ENDPOINT", DESCRIPTION = "ENDPOINT", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  }
            };
            return lstPermissionType;
        }

        /// <summary>
        /// Fake peoples 
        /// </summary>
        /// <returns>List of peoples</returns>
        public static List<Person> AddPerson()
        {
            List<Person> lstPerson = new()
            {
                new Person() { NAME = "Fredel", MIDDLE_NAME = "Reynel", LASTNAME_PAT = "Pacheco", LASTNAME_MAT = "Caamal", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, EMAIL = "OXXO@oxxo.com"  },
                new Person() { NAME = "Luis", MIDDLE_NAME = "Alberto", LASTNAME_PAT = "Caballero", LASTNAME_MAT = "Cruz", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, EMAIL = "OXXO@oxxo.com"  },
                new Person() { NAME = "Francisco", MIDDLE_NAME = "Ivan", LASTNAME_PAT = "Ramirez", LASTNAME_MAT = "Alcaraz", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow, EMAIL = "OXXO@oxxo.com"  },
                new Person() { NAME = "Fernando", MIDDLE_NAME = "Abrego", LASTNAME_PAT = "Rodriguez", LASTNAME_MAT = string.Empty, IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow , EMAIL = "OXXO@oxxo.com" },
            };

            return lstPerson;
        }

        /// <summary>
        /// Fake operator status 
        /// </summary>
        /// <returns>List Operators status</returns>
        public static List<OperatorStatus> AddStatusData()
        {
            List<OperatorStatus> lstOperatorStatus = new()
            {
                new OperatorStatus() { CODE="AC", NAME = "ACTIVE", DESCRIPTION = "STATUS FAKE", IS_ACTIVE = true, LCOUNT = 1, CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398"), CREATED_DATETIME = DateTime.UtcNow  },
            };

            return lstOperatorStatus;
        }

        public static List<ApiKey> AddApiKeys()
        {
            return new List<ApiKey>()
            {
                new ApiKey() {
                    EXTERNAL_APPLICATION_ID = GuidSessionToken,
                    API_KEY_ID = 1,
                    API_KEY = Api_Key,
                    EXPIRATION_TIME = DateTime.UtcNow,
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }

        public static List<StorePlace> AddStorePlaces()
        {
            return new List<StorePlace>()
            {
                new StorePlace() {
                    STORE_ID_SOURCE = 1,
                    CR_PLACE = "CrPlace1",
                    CR_STORE = "CrStore1",
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                },
                new StorePlace() {
                    STORE_ID_SOURCE = 2,
                    CR_PLACE = "CrPlace2",
                    CR_STORE = "CrStore2",
                    IS_ACTIVE= true,
                    CREATED_DATETIME= DateTime.UtcNow,
                    CREATED_BY_OPERATOR_ID = Guid.Parse("8B32FA3D-3A48-4B91-8325-1D14F73A6398")
                }
            };
        }
        #endregion
    }
}
