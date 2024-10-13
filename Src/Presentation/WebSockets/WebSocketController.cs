using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Application.Utils;

namespace SysgamingApi.Src.Presentation.WebSockets
{
    [Route("api/ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly INotifcationService _notificationService;

        public WebSocketController(INotifcationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var context = ControllerContext.HttpContext;

            if (!context.WebSockets.IsWebSocketRequest)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            _ = _notificationService.CreateConnection(socket);

            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }


    }
}
