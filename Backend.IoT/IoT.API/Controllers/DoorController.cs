using IoT.Application.Common;
using IoT.Application.Devices.Commands.Create;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/door")]
[Authorize]
public class DoorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppDbContext _db;

    public DoorController(
        IMediator mediator,
        IAppDbContext db)
    {
        _mediator = mediator;
        _db = db;
    }

    public sealed class DoorCommandRequest
    {
        public DeviceCommandType Command { get; init; }
        public int? Percentage { get; init; }
    }

    [HttpPost("command")]
    public async Task<IActionResult> SendCommand(
        [FromBody] DoorCommandRequest request,
        CancellationToken ct)
    {

        if (request.Command == DeviceCommandType.Vent)
        {
            if (request.Percentage is null or < 1 or > 99)
                return BadRequest("Vent requires percentage 1–99.");
        }

        // ✅ SINGLE DEVICE SYSTEM
        var device = await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        if (device == null)
            return Problem("No device registered in the system.");

        await _mediator.Send(
            new CreateDeviceCommandCommand(
                device.Id,          // ← GUID
                request.Command,
                request.Percentage),
            ct);

        return Accepted();
    }
}
