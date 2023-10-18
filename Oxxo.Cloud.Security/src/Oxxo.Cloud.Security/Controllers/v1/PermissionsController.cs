#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    28/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Permissions.Commands.AssigmentPermissionstorole;
using Oxxo.Cloud.Security.Application.Permissions.Commands.CreatePermissions;
using Oxxo.Cloud.Security.Application.Permissions.Commands.DeletePermissions;
using Oxxo.Cloud.Security.Application.Permissions.Commands.UpdatePermissions;
using Oxxo.Cloud.Security.Application.Permissions.Queries;
using Oxxo.Cloud.Security.Application.Permissions.Queries.MenuPerModuleQuery;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;

namespace Oxxo.Cloud.Security.WebUI.Controllers.v1
{
    [Authorize]
    [ApiVersion("1")]
    [SwaggerTag(GlobalConstantSwagger.TAGPERMISSIONS)]
    public class PermissionsController : ApiControllerBase
    {
        private new IMediator Mediator { get; set; }
        private readonly ILogService logService;

        public PermissionsController(IMediator mediator, ILogService logService)
        {
            Mediator = mediator;
            this.logService = logService;
        }

        /// <summary>
        /// This method is using to create new permission in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>       
        [Route(GlobalConstantHelpers.METHODPERMISSIONCREATE)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.CREATEPERMISSIONS, Description = GlobalConstantSwagger.DESCCREATEPERMISSIONS)]
        public async Task<IActionResult> Create([FromBody] CreatePermissionsCommand permissionCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                permissionCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                permissionCommand.UserIdentification = userIdentification;    
                permissionCommand.Identification = identification;
                await Mediator.Send(permissionCommand ?? new());                
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONCREATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPICREATE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONCREATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPICREATE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to update permissions in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>        
        [Route(GlobalConstantHelpers.METHODPERMISSIONUPDATE)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.UPDATEPERMISSIONS, Description = GlobalConstantSwagger.DESCUPDATEPERMISSIONS)]
        public async Task<IActionResult> Update([FromBody] UpdatePermissionsCommand permissionCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;           
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                permissionCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                permissionCommand.UserIdentification = userIdentification;
                permissionCommand.Identification = identification;
                await Mediator.Send(permissionCommand ?? new());
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONUPDATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIUPDATE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONUPDATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIUPDATE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to delete permissions in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>        
        [Route(GlobalConstantHelpers.METHODPERMISSIONDELETE)]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.DELETEPERMISSIONS, Description = GlobalConstantSwagger.DESCDELETEPERMISSIONS)]
        public async Task<IActionResult> Delete([FromBody] DeletePermissionsCommand permissionCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                permissionCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                permissionCommand.UserIdentification = userIdentification;  
                permissionCommand.Identification = identification;
                await Mediator.Send(permissionCommand ?? new());
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONUPDATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIDELETE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONDELETE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIDELETE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to get list permissions in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>       
        [Route(GlobalConstantHelpers.METHODPERMISSIONGET)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PermissionGetDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETPERMISSIONS, Description = GlobalConstantSwagger.DESCGETPERMISSIONS)]
        public async Task<IActionResult> Get(int pageNumber, int itemsNumber)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;           
            try
            {                
                PermissionsQuery permissionCommand = new()
                {
                    PageNumber = pageNumber,
                    ItemsNumber = itemsNumber,
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(permissionCommand));               
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGET, LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGET
                    , LogTypeEnum.Error, userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to get list permissions in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>        
        [Route(GlobalConstantHelpers.METHODPERMISSIONGETBYID)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETPERMISSIONBYID, Description = GlobalConstantSwagger.DESCGETPERMISSIONBYID)]
        public async Task<IActionResult> GetPermissionById(int permissionId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {                
                PermissionsQueryById permissionCommand = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    permissionID = permissionId,
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(permissionCommand));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETBYID
                    , LogTypeEnum.Error, userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, string.Concat(ex.GetException(), ex.ErrorsMessage)
                    , Request.Path.Value), GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETBYID
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to get list permissions in database
        /// </summary>
        /// <param name="permissionCommand">Request command</param>
        /// <returns>logic value to indicated the result to operation</returns>       
        [Route(GlobalConstantHelpers.METHODPERMISSIONGETBYROLEID)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETPERMISSIONBYIDROL, Description = GlobalConstantSwagger.DESCGETPERMISSIONBYIDROL)]
        public async Task<IActionResult> GetPermissionByIdRol(int roleId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {                
                PermissionsQueryByIdRol permissionCommand = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    roleId = roleId,
                    UserIdentification = userIdentification
                };
                return Ok(await Mediator.Send(permissionCommand));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETBYROLEID
                    , LogTypeEnum.Error, userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETBYROLEID
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }
        
        [Route(GlobalConstantHelpers.METHODASSINGPERMISSIONTOROLE)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.ASSIGNPERMISSIONTOROLE, Description = GlobalConstantSwagger.DESCASSIGNPERMISSIONTOROLE)]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssigmentPermissionToRoleCommand assigmentPermissionToRoleCommand) 
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                assigmentPermissionToRoleCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                assigmentPermissionToRoleCommand.UserIdentification = userIdentification;  
                assigmentPermissionToRoleCommand.Identification = identification;
                await Mediator.Send(assigmentPermissionToRoleCommand);              
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODASSINGPERMISSIONTOROLE,
                    LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORASSINGPERMISSIONTOROLEAPI,
                    string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value), GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODPERMISSIONGETBYROLEID,
                    LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, ex.GetException(),
                    Request.Path.Value), GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// This method is using to get menu of Front End per module
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <returns>logic value to indicated the result to operation</returns>       
        [Route(GlobalConstantHelpers.METHODPERMISSIONGETMENUPERMODULE)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.MENUPERMODULE, Description = GlobalConstantSwagger.DESMENYPERMODULE)]
        public async Task<IActionResult> MenuPerModule(string moduleName, int? parentId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;
            string userId = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
            
            try
            {
                MenuPerModuleQuery menuPerModuleQuery = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserId = Guid.Parse(userId),
                    ModuleName = moduleName,
                    ParentId = parentId,
                    UserIdentification = userIdentification
                };
                return Ok(await Mediator.Send(menuPerModuleQuery));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETMENUPERMODULE
                    , LogTypeEnum.Error, userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORMENUPERMODULE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION
                    , GlobalConstantHelpers.METHODPERMISSIONGETMENUPERMODULE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORMENUPERMODULE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }
    }
}
