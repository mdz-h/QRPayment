using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Oxxo.Cloud.Security.WebUI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender mediator = null!;
        protected ISender Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
