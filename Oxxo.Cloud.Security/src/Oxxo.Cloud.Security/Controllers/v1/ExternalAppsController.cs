#region File Information
//===============================================================================
// Author:  Roberto Carlos Prado Montes (NEORIS).
// Date:    2022-12-06.
// Comment: Controller of External Apps.
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
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.AssignExternalAppsToRole;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.CreateExternalApps;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteApiKey;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.DeleteExternalApps;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.GenerateApiKey;
using Oxxo.Cloud.Security.Application.ExternalApps.Commands.UpdateExternalApps;
using Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;
using Const = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantHelpers;
using ConstLog = Oxxo.Cloud.Security.Domain.Consts.GlobalConstantMessages;


namespace Oxxo.Cloud.Security.WebUI.Controllers.v1
{
    [Authorize]
    [ApiVersion("1")]
    [SwaggerTag(GlobalConstantSwagger.TAGEXTERNALAPPS)]
    public class ExternalAppsController : ApiControllerBase
    {
        #region Properties
        /// <summary>
        /// Contract of Mediator
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Contract of LogService
        /// </summary>
        private readonly ILogService _logService;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator">Contract of Mediator</param>
        /// <param name="logService">Contract of Logervice</param>
        public ExternalAppsController(IMediator mediator, ILogService logService)
        {
            _mediator = mediator;
            _logService = logService;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Get ExternalApps
        /// </summary>
        /// <param name="itemsNumber">Items Number</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="identifier">Id</param>        
        [HttpGet]
        [Route(Const.METHODGET)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ExternalAppsResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETEXTERNALAPPS, Description = GlobalConstantSwagger.DESCGETEXTERNALAPPS)]
        public async Task<IActionResult> Get([FromQuery] int itemsNumber, [FromQuery] int pageNumber, [FromQuery] string identifier)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;           
            try
            {               
                return Ok(await Mediator.Send(new GetExternalAppsQuery
                {
                    Identifier = identifier,
                    ItemsNumber = itemsNumber,
                    PageNumber = pageNumber,
                    UserIdentification = userIdentification                   
                }));  
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTEXTERNALAPPSGET,
                    Const.METHODGET,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.EXTERNALAPPS_ERROR_API_GET, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value),
                    Const.EXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.EVENTEXTERNALAPPSGET,
                    Const.METHODGET,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.EXTERNALAPPS_ERROR_API_GET, ex.GetException(), Request.Path.Value),
                    Const.EXTERNALAPPSCONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Create ExternalApps
        /// </summary>
        /// <param name="createExternalAppsCommand">CreateExternalAppsCommand object</param>        
        [HttpPost]
        [Route(Const.METHODCREATE)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.CREATEEXTERNALAPPS, Description = GlobalConstantSwagger.DESCCREATEEXTERNALAPPS)]
        public async Task<IActionResult> Create([FromBody] CreateExternalAppsCommand createExternalAppsCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                createExternalAppsCommand.UserIdentification = userIdentification;        
                createExternalAppsCommand.Identification = identification;
                await _mediator.Send(createExternalAppsCommand);               
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTCREATEEXTERNALAPPLICATION,
                     Const.METHODCREATE,
                     LogTypeEnum.Error,
                     userIdentification,
                     string.Concat(ConstLog.LOG_ERROR_EXTERNALAPPSCREATE, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value),
                     Const.NAMEDEVICECONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                var exception = ex.GetBaseException();
                await _logService.Logger(Const.EVENTCREATEEXTERNALAPPLICATION,
                    Const.METHODCREATE,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.LOG_ERROR_EXTERNALAPPSCREATE, exception.GetException(), Request.Path.Value),
                    Const.NAMEDEVICECONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for external app modification.
        /// </summary>
        /// <param name="externalAppsCommand">Contains the main values for a external app</param>
        /// <returns>status Code of operation result</returns>
        [HttpPut]
        [Route(Const.METHODUPDATEEXTERNALAPPS)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.UPDATEEXTERNALAPPS, Description = GlobalConstantSwagger.DESCUPDATEEXTERNALAPPS)]

        public async Task<IActionResult> Update([FromBody] UpdateExternalAppsCommand externalAppsCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                externalAppsCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                externalAppsCommand.UserIdentification = userIdentification;
                externalAppsCommand.Identification = identification;
                await Mediator.Send(externalAppsCommand);               
                return Ok();
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTMETHODUPDATEEXTERNALAPPS,
                    Const.METHODUPDATEEXTERNALAPPS,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.LOGERROREXTERNALAPPSUPDATEAPI, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value),
                    Const.NAMEEXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                var exception = ex.GetBaseException();
                await _logService.Logger(Const.EVENTMETHODUPDATEEXTERNALAPPS,
                    Const.METHODUPDATEEXTERNALAPPS,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.LOGERROREXTERNALAPPSUPDATEAPI, ex.GetException(), Request.Path.Value),
                    Const.NAMEEXTERNALAPPSCONTROLLER);
                throw exception;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for external app deletion.
        /// </summary>
        /// <param name="externalAppsCommand">Contains the main values for a external app</param>
        /// <returns>status Code of operation result</returns>       
        [HttpDelete]
        [Route(Const.METHODDELETEEXTERNALAPPS)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.DELETEEXTERNALAPPS, Description = GlobalConstantSwagger.DESCDELETEEXTERNALAPPS)]
        public async Task<IActionResult> Delete(string externalAppId)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                DeleteExternalAppsCommand externalAppsCommand = new()
                {
                    BearerToken = Request.Headers[HeaderNames.Authorization],
                    UserIdentification = userIdentification,
                    ExternalAppId = externalAppId,
                    Identification = identification
                };   
                await Mediator.Send(externalAppsCommand);
                return Ok();
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTMETHODDELETEEXTERNALAPPS,
                    Const.METHODDELETEEXTERNALAPPS,
                    LogTypeEnum.Error,
                    userIdentification,
                    string.Concat(ConstLog.LOGERROREXTERNALAPPSDELETEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value),
                    Const.NAMEEXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.EVENTMETHODDELETEEXTERNALAPPS,
                    Const.METHODDELETEEXTERNALAPPS,
                    LogTypeEnum.Error,
                    string.Concat(ConstLog.LOGERROREXTERNALAPPSDELETEAPI, ex.GetException(), Request.Path.Value),
                    Const.NAMEEXTERNALAPPSCONTROLLER);

                throw;
            }
        }

        /// <summary>
        /// Generate API Key
        /// </summary>
        /// <param name="generateApiKeyCommand">Request parameters</param>
        /// <returns>API KEY values</returns>       
        [HttpPost]
        [Route(Const.METHODEXTERNALAPPGENERATE)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiKeyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GENERATEAPIKEYEXTERNALAPPS, Description = GlobalConstantSwagger.DESGENERATEAPIKEYEXTERNALAPPS)]
        public async Task<IActionResult> GenerateKey([FromBody] GenerateApiKeyCommand generateApiKeyCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                generateApiKeyCommand.BearerToken = Request.Headers[HeaderNames.Authorization];
                generateApiKeyCommand.UserIdentification = userIdentification;
                generateApiKeyCommand.Identification = identification;
                ApiKeyDto ApiKey = await Mediator.Send(generateApiKeyCommand);              
                return Created(string.Empty, ApiKey);
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTPOSTEXTERNALAPPS
                    , Const.EVENTMETHODGENERATEAPIKEYEXTERNALAPPS
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(ConstLog.LOGERRORGENERATEAPIEXTERNALAPP, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.EVENTPOSTEXTERNALAPPS
                    , Const.EVENTMETHODGENERATEAPIKEYEXTERNALAPPS
                    , LogTypeEnum.Error
                    , string.Concat(ConstLog.LOGERRORGENERATEAPIEXTERNALAPP, ex.GetException(), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);

                throw;
            }
        }

        /// <summary>
        /// Delete an API Key
        /// </summary>
        /// <param name="generateApiKeyCommand">Request parameters</param>
        /// <returns>Status Code</returns>        
        [HttpDelete]
        [Route(Const.METHODEXTERNALAPPDELETE)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.DELETEAPIKEYEXTERNALAPPS, Description = GlobalConstantSwagger.DESDELETEAPIKEYEXTERNALAPPS)]
        public async Task<IActionResult> DeleteKey([FromBody] DeleteApiKeyCommand command)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                command.BearerToken = Request.Headers[HeaderNames.Authorization];
                command.UserIdentification = userIdentification;    
                command.Identification = identification;
                await Mediator.Send(command);  
                return Ok();
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.METHODEXTERNALAPPDELETE
                    , Const.EVENTMETHODDELETEAPIKEY                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(ConstLog.LOGERRORDELETEAPIKEY, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.METHODEXTERNALAPPDELETE
                    , Const.EVENTMETHODDELETEAPIKEY
                    , LogTypeEnum.Error
                    , string.Concat(ConstLog.LOGERRORDELETEAPIKEY, ex.GetException(), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);

                throw;
            }
        }


        /// <summary>
        /// Generate relationship between external application and role
        /// </summary>
        /// <param name="assignExternalApps">Request parameters</param>
        /// <returns>Relationship between external application and role</returns>        
        [HttpPost]
        [Route(Const.EVENTPOSTEXTERNALAPPSASSIGN)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.ASSINGNEXTERNALAPPS, Description = GlobalConstantSwagger.DESCASSINGNEXTERNALAPPS)]
        public async Task<IActionResult> AssignExternalAppsToRole([FromBody] AssignExternalAppsToRoleCommand assignExternalApps)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == Const.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                assignExternalApps.BearerToken = Request.Headers[HeaderNames.Authorization];
                assignExternalApps.UserIdentification = userIdentification;
                assignExternalApps.Identification = identification;
                await Mediator.Send(assignExternalApps);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await _logService.Logger(Const.EVENTPOSTEXTERNALAPPSASSIGN
                    , Const.METHODASSIGNTORLEEXTERNALAPPS
                    , LogTypeEnum.Error
                    , userIdentification
                    , string.Concat(ConstLog.LOGERROREXTERNALAPPTOASSIGN, string.Concat(ex.GetException(), ex.ErrorsMessage), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await _logService.Logger(Const.EVENTPOSTEXTERNALAPPSASSIGN
                    , Const.METHODASSIGNTORLEEXTERNALAPPS
                    , LogTypeEnum.Error
                    , string.Concat(ConstLog.LOGERROREXTERNALAPPTOASSIGN, ex.GetException(), Request.Path.Value)
                    , Const.NAMEEXTERNALAPPSCONTROLLER);

                throw;
            }
        }
        #endregion 
    }
}