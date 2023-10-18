#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-11-28.
// Comment: Controller device.
//===============================================================================
#endregion
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Application.Device.Commands.Enabled;
using Oxxo.Cloud.Security.Application.Device.Commands.Register;
using Oxxo.Cloud.Security.Application.Device.Queries;
using Oxxo.Cloud.Security.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;
using ValidationException = Oxxo.Cloud.Security.Application.Common.Exceptions.ValidationException;


namespace Oxxo.Cloud.Security.WebUI.Controllers.v1
{
    [ApiVersion("1")]
    [SwaggerTag(GlobalConstantSwagger.TAGDEVICE)]
    public class DevicesController : ApiControllerBase
    {
        private new IMediator Mediator { get; set; }
        private readonly ILogService logService;
        public DevicesController(IMediator mediator, ILogService logService)
        {
            this.Mediator = mediator;
            this.logService = logService;
        }

        /// <summary>
        /// Its executes the handler responsible for register devices.
        /// </summary>
        /// <param name="registerDeviceCommand">Object with the data needed to register the device</param>
        /// <returns></returns>
        [HttpPost]
        [Route(GlobalConstantHelpers.METHODREGISTER)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.REGISTERDEVICE, Description = GlobalConstantSwagger.DESCREGISTERDEVICE)]
        public async Task<IActionResult> Register([FromBody] RegisterDeviceCommand registerDeviceCommand)
        {
            try
            {
                registerDeviceCommand.Identification = Environment.GetEnvironmentVariable(GlobalConstantHelpers.URLOPERATORIDDEFAULT) ?? string.Empty;
                await Mediator.Send(registerDeviceCommand);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHODREGISTER, LogTypeEnum.Error, string.Empty,
                string.Concat(GlobalConstantMessages.LOGERRORREGISTERDEVICEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEDEVICECONTROLLER);
                throw;
            }
            catch (Exception ex)
            {                
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODREGISTERDEVICE, GlobalConstantHelpers.METHODREGISTER, LogTypeEnum.Error, string.Empty, string.Concat(GlobalConstantMessages.LOGERRORREGISTERDEVICEAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEDEVICECONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for get devices.
        /// </summary>
        /// <param name="itemsNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="deviceIdentifier"></param>
        /// <returns>Gets the data of the devices.</returns>
        [Authorize]
        [HttpGet]
        [Route(GlobalConstantHelpers.METHODGETDEVICES)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.GETDEVICE, Description = GlobalConstantSwagger.DESCGETDEVICE)]
        public async Task<IActionResult> Get([FromQuery] int itemsNumber, [FromQuery] int pageNumber, [FromQuery] string deviceIdentifier)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {                
                return Ok(await Mediator.Send(new GetDevicesQuery { ItemsNumber = itemsNumber, PageNumber = pageNumber, DeviceIdentifier = deviceIdentifier, UserIdentification = userIdentification }));
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETDEVICES, GlobalConstantHelpers.METHODGETDEVICES, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORGETDEVICESAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEDEVICECONTROLLER);
                throw;
            }
            catch (Exception ex)
            {                
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODGETDEVICES, GlobalConstantHelpers.METHODGETDEVICES, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGERRORGETDEVICESAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEDEVICECONTROLLER);
                throw;
            }
        }

        /// <summary>
        /// Its executes the handler responsible for Enabled/Disabled devices.
        /// </summary>
        /// <param name="enabledDeviceCommand"></param>
        /// <returns>Gets the data of the devices.</returns>
        [Authorize]
        [HttpPut]
        [Route(GlobalConstantHelpers.METHODENABLED)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = GlobalConstantSwagger.ENABLEDEVICE, Description = GlobalConstantSwagger.DESCENABLEDEVICE)]

        public async Task<IActionResult> Enabled([FromBody] EnabledDeviceCommand enabledDeviceCommand)
        {
            string userIdentification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.USERIDENTIFICATION).Value;            
            try
            {
                string identification = Request.HttpContext.User.Claims.First(c => c.Type == GlobalConstantHelpers.IDENTIFICATION).Value;
                enabledDeviceCommand.UserIdentification = userIdentification;
                enabledDeviceCommand.Identification = identification;
                await Mediator.Send(enabledDeviceCommand);
                return Ok();
            }
            catch (ValidationException ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODENABLEDDEVICE, GlobalConstantHelpers.METHODENABLED, LogTypeEnum.Error, userIdentification,
                  string.Concat(GlobalConstantMessages.LOGERRORENABLEDDEVICEAPI, ex.GetException(), ex.ErrorsMessage, Request.Path.Value), GlobalConstantHelpers.NAMEAUTHENTICATECONTROLLER);
                throw;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODENABLEDDEVICE, GlobalConstantHelpers.METHODENABLED, LogTypeEnum.Error, userIdentification,
                    string.Concat(GlobalConstantMessages.LOGENDENABLEDDEVICEAPI, ex.GetException(), Request.Path.Value), GlobalConstantHelpers.NAMEENABLEDDEVICECONTROLLER);
                throw;
            }
        }        
    }
}
