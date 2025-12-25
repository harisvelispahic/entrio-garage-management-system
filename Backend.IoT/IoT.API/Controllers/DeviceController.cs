using IoT.API.Security;
using Microsoft.AspNetCore.Mvc;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/device")]
public class DeviceController : ControllerBase
{
    [DeviceAuthorize]
    [HttpPost("ping")]
    public IActionResult Ping(
    [FromHeader(Name = "X-Device-Key")] string deviceKey)
    {
        return Ok("DEVICE AUTH OK");
    }

}
