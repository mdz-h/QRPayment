#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-24.
// Comment: Controller Roles.
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
using Oxxo.Cloud.Security.Application.Roles.Commands.AssignRoleToUser;
using Oxxo.Cloud.Security.Application.Roles.Commands.CreateRoles;
using Oxxo.Cloud.Security.Application.Roles.Commands.DeleteRoles;
using Oxxo.Cloud.Security.Application.Roles.Commands.UpdateRoles;
using Oxxo.Cloud.Security.Application.Roles.Queries;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;

namespace Oxxo.Cloud.Security.WebUI.Controllers.v1
{
    [Authorize]
    [ApiVersion("1")]
    [SwaggerTag(GlobalConstantSwagger.TAGROLE)]
    public class RolesController : ApiControllerBase
    {
        private new IMediator Mediator { get; set; }
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the application db context and log instance.
        /// </summary>
        /// <param name="mediator">"Inject" mediator instance</param>
        /// <param name="logService">"Inject" the log instance</param>
        public RolesController(IMediator mediator, ILogService logService)
        {
            this.Mediator = mediator;
            this.logService = logService;
        }

        /// <summary>
        /// Its executes the handler responsible for workgroups creation.
        /// </summary>
        /// <param name="RolesCommand">Contains the main values for a workgroup</param>
        /// <returns>status Code of operation result</returns>
        [HttpPost]
        [Route(GlobalConstantHelpers.METHODCREATEWORKGROUP)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.CREATEROLE, Description = GlobalConstantSwagger.DESCCREATEROLE)]
        public async Task<IActionResult> Create([FromBody] CreateRolesCommand RolesCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                RolesCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                RolesCommand.UserIdentification = userIdentification;
                RolesCommand.Identification = identification;
                await Mediator.Send(RolesCommand);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODCREATEWORKGROUP, GlobalConstantHelpers.METHODCREATEWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPCREATEAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODCREATEWORKGROUP, GlobalConstantHelpers.METHODCREATEWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPCREATEAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for workgroups update.
        /// </summary>
        /// <param name="RolesCommand">Contains the main values for a workgroup</param>
        /// <returns>status Code of operation result</returns>
        [HttpPut]
        [Route(GlobalConstantHelpers.METHODUPDATEWORKGROUP)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.UPDATEROLE, Description = GlobalConstantSwagger.DESCUPDATEROLE)]
        public async Task<IActionResult> Update([FromBody] UpdateRolesCommand RolesCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                RolesCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                RolesCommand.UserIdentification = userIdentification;
                RolesCommand.Identification = identification;
                await Mediator.Send(RolesCommand);
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODUPDATEWORKGROUP, GlobalConstantHelpers.METHODUPDATEWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPUPDATEAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODUPDATEWORKGROUP, GlobalConstantHelpers.METHODUPDATEWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPUPDATEAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for workgroups update.
        /// </summary>
        /// <param name="RolesCommand">Contains the main values for a workgroup</param>
        /// <returns>status Code of operation result</returns>
        [HttpDelete]
        [Route(GlobalConstantHelpers.METHODDELETEWORKGROUP)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.DELETEROLE, Description = GlobalConstantSwagger.DESCDELETEROLE)]
        public async Task<IActionResult> Delete(int roleId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                DeleteRolesCommand request = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    RoleId = roleId,
                    UserIdentification = userIdentification,
                    Identification = identification
                };
                await Mediator.Send(request);
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUP, GlobalConstantHelpers.METHODDELETEWORKGROUP, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPDELETEAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUP, GlobalConstantHelpers.METHODDELETEWORKGROUP, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPDELETEAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for workgroups update.
        /// </summary>
        /// <param name="RolesCommand">Contains the main values for a workgroup</param>
        /// <returns>status Code of operation result</returns>
        [HttpDelete]
        [Route(GlobalConstantHelpers.METHODDELETEWORKGROUPUSER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.DELETEROLETOUSER, Description = GlobalConstantSwagger.DESCDELETEROLETOUSER)]
        public async Task<IActionResult> DeleteRoleToUser(string userId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                DeleteUserRoleCommand request = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserId = userId,
                    UserIdentification = userIdentification,
                    Identification = identification
                };
                await Mediator.Send(request);
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUPUSER, GlobalConstantHelpers.METHODDELETEWORKGROUPUSER, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORUSERWORKGROUPDELETEAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODDELETEWORKGROUPUSER, GlobalConstantHelpers.METHODDELETEWORKGROUPUSER, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORUSERWORKGROUPDELETEAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for get workgroups.
        /// </summary>
        /// <param name="skip">Value for skips elements up to a specified position</param>
        /// <param name="take">Value for takes elements up to a specified position</param>
        /// <returns>status Code of operation result</returns>
        [HttpGet]
        [Route(GlobalConstantHelpers.METHODGETWORKGROUP)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RolesDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETROLE, Description = GlobalConstantSwagger.DESCGETROLE)]
        public async Task<IActionResult> Get(int skip, int take)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {                
                RolesQuery request = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    Skip = skip,
                    Take = take,
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(request));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPGETAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUP, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPGETAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for get a specified role by id.
        /// </summary>
        /// <param name="roleId">Value for a specified role id</param>        
        /// <returns>role information</returns>
        [HttpGet]
        [Route(GlobalConstantHelpers.METHODGETWORKGROUPBYID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RolesDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETROLEBYID, Description = GlobalConstantSwagger.DESCGETROLEBYID)]
        public async Task<IActionResult> GetById(int roleId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;

            try
            {               
                RolesByIdQuery request = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    RoleId = roleId,
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(request));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUPBYID, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPGETBYIDAPI, string.Concat(ex.Message, ex.ErrorsMessage, ex.StackTrace), Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUPBYID, LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORWORKGROUPGETBYIDAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for get a specified role by id.
        /// </summary>
        /// <param name="roleId">Value for a specified role id</param>        
        /// <returns>role information</returns>
        [HttpGet]
        [Route(GlobalConstantHelpers.METHODGETWORKGROUPBYUSERID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RolesDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETROLEBYUSERID, Description = GlobalConstantSwagger.DESCGETROLEBYUSERID)]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;
            try
            {                
                RolesByUserIdQuery request = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserId = userId,
                    UserIdentification = userIdentification
                };
                return Ok(await Mediator.Send(request));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUPBYID, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORUSERWORKGROUPLINKGETAPI, string.Concat(ex.Message, ex.ErrorsMessage), Request.Path.Value, ex.StackTrace), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETWORKGROUP, GlobalConstantHelpers.METHODGETWORKGROUPBYID, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORUSERWORKGROUPLINKGETAPI, ex.Message, ex.StackTrace, Request.Path.Value), GlobalConstantHelpers.NAMEROLESCONTROLLER);
                throw;
            }
        }

        [HttpPost]
        [Route(GlobalConstantHelpers.METHODASSINGROLETOUSER)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.ASSIGNROLETOUSER, Description = GlobalConstantSwagger.DESCASSIGNROLETOUSER)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand assignRoleToUserCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                assignRoleToUserCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                assignRoleToUserCommand.UserIdentification = userIdentification;
                assignRoleToUserCommand.Identification = identification;
                await Mediator.Send(assignRoleToUserCommand);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODASSINGPERMISSIONTOROLE, LogTypeEnum.Error, userIdentification,
                string.Concat(GlobalConstantMessages.LOGERRORASSINGPERMISSIONTOROLEAPI, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value), GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPERMISSION, GlobalConstantHelpers.METHODPERMISSIONGETBYROLEID,
                LogTypeEnum.Error, userIdentification, string.Concat(GlobalConstantMessages.LOGERRORPERMISSIONAPIGET, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEPERMISSIONCONTROLLER);
                throw;
            }
        }
    }
}
