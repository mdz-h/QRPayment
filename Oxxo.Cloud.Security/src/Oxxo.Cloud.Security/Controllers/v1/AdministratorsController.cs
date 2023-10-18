#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrator.
//===============================================================================
#endregion

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword;
using Oxxo.Cloud.Security.Application.Administrators.Commands.CreateAdministrators;
using Oxxo.Cloud.Security.Application.Administrators.Commands.DeleteAdministrators;
using Oxxo.Cloud.Security.Application.Administrators.Commands.UpdateAdministrators;
using Oxxo.Cloud.Security.Application.Administrators.Queries;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;

namespace Oxxo.Cloud.Security.WebUI.Controllers.v1
{
    [Authorize]
    [ApiVersion("1")]
    [SwaggerTag(GlobalConstantSwagger.TAGADMINISTATOR)]
    public class AdministratorsController : ApiControllerBase
    {
        private new IMediator Mediator { get; set; }
        private readonly ILogService logService;

        /// <summary>
        /// Constructor Class
        /// </summary>
        /// <param name="mediator">patter design mediator</param>
        /// <param name="logService">interce Log</param>
        public AdministratorsController(IMediator mediator, ILogService logService)
        {
            Mediator = mediator;
            this.logService = logService;
        }

        /// <summary>
        /// Delete controller
        /// </summary>
        /// <param name="administratorsCommand">administrators params</param>
        /// <returns>Return status code</returns>
        /// 
        [Route(GlobalConstantHelpers.METHODADMINISTRATORSDELETE)]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary =GlobalConstantSwagger.DELETEADMINISTATOR, Description =GlobalConstantSwagger.DESCDELETEADMINISTATOR)]
        public async Task<IActionResult> Delete([FromBody] DeleteAdministratorsCommand administratorsCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                administratorsCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                administratorsCommand.UserIdentification = userIdentification;
                administratorsCommand.Identification = identification;
                await Mediator.Send(administratorsCommand ?? new());
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSDELETE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIDELETE, string.Concat(ex.Message, ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSDELETE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIDELETE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for create administrator.
        /// </summary>
        /// <param name="createAdministratorCommand"></param>
        /// <returns></returns>        
        [HttpPost]
        [Route(GlobalConstantHelpers.METHODADMINISTRATORSCREATE)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.CREATEADMINISTATOR, Description =GlobalConstantSwagger.DESCCREATEADMINISTATOR)]
        public async Task<IActionResult> Post([FromBody] CreateAdministratorCommand createAdministratorCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                createAdministratorCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                createAdministratorCommand.UserIdentification = userIdentification;
                createAdministratorCommand.Identification = identification;
                await Mediator.Send(createAdministratorCommand);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODPOSTADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSCREATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORCREATEADMINISTRATOR, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSCREATE
                    , LogTypeEnum.Error
                    , string.Concat(GlobalConstantMessages.LOGERRORCREATEADMINISTRATOR, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Delete controller
        /// </summary>
        /// <param name="administratorsCommand">administrators params</param>
        /// <returns>Return status code</returns>       
        [Route(GlobalConstantHelpers.METHODADMINISTRATORSUPDATE)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.UPDATEADMINISTATOR,Description =GlobalConstantSwagger.DESCUPDATEADMINISTATOR)]
        public async Task<IActionResult> Update([FromBody] UpdateAdministratorsCommand administratorsCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                administratorsCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                administratorsCommand.UserIdentification = userIdentification;
                administratorsCommand.Identification = identification;
                await Mediator.Send(administratorsCommand ?? new());               
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSUPDATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIUPDATE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSUPDATE
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIUPDATE, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Get controller
        /// </summary>
        /// <param name="administratorsCommand">administrators parameters</param>
        /// <returns>Return status code</returns>        
        [Route(GlobalConstantHelpers.METHODADMINISTRATORSGET)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AdministratorGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETADMINISTATOR, Description = GlobalConstantSwagger.DESGETADMINISTATOR)]
        public async Task<IActionResult> Get(int pageNumber, int itemsNumber)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {                
                AdministratorsQuery administratorsCommand = new()
                {
                    PageNumber = pageNumber,
                    ItemsNumber = itemsNumber,
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(administratorsCommand ?? new()));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGET
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGET
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Get By Id controller
        /// </summary>
        /// <param name="administratorsCommand">administrators parameters</param>
        /// <returns>Return status code</returns>       
        [Route(GlobalConstantHelpers.METHODADMINISTRATORSGETBYID)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdministratorGetDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETADMINISTATORBYID, Description = GlobalConstantSwagger.DESCGETADMINISTATORBYID)]
        public async Task<IActionResult> GetAdministratorsById(string userId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;           

            try
            {               
                AdministratorsQueryById administratorsCommand = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserId = userId,
                    UserIdentification = userIdentification                   
                };
                return Ok(await Mediator.Send(administratorsCommand ?? new()));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGETBYID
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHODADMINISTRATORSGETBYID
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORGETADMINISTRATORSAPIGET, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for change password for Administrators.
        /// </summary>
        /// <param name="changePasswordCommand">Params for Change Password</param>
        /// <returns>Return status code</returns>       
        [Route(GlobalConstantHelpers.ENDPOINTCHANGEPASSWORDADMINISTRATOR)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.CHANGEPASSADMINISTRATOR, Description = GlobalConstantSwagger.DESCCHANGEPASSADMINISTRATOR)]
        public async Task<IActionResult> PostChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            

            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                changePasswordCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                changePasswordCommand.UserIdentification = userIdentification;
                changePasswordCommand.Identification = identification;
                await Mediator.Send(changePasswordCommand ?? new());               
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODCHANGEPASSWORD
                    , GlobalConstantHelpers.METHODADMINISTRATORSCHANGEPASSWORD
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORCHANGEPASSWORD, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODCHANGEPASSWORD
                    , GlobalConstantHelpers.METHODADMINISTRATORSCHANGEPASSWORD
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORCHANGEPASSWORD, ex.GetException(), Request.Path.Value)
                    , GlobalConstantHelpers.NAMEADMINISTRATORSCONTROLLER);
                throw;
            }
        }
    }
}