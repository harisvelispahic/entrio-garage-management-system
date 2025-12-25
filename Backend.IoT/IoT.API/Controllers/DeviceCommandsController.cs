using IoT.API.Security;
using IoT.Application.Common;
using IoT.Application.Devices.Commands.GetPending;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoT.API.Controllers;

[Route("api/device/commands")]
[ApiController]
public class DeviceCommandsController : ControllerBase
{
    private readonly IAppDbContext _db;
    private readonly IMediator _mediator;

    public DeviceCommandsController(IAppDbContext db, IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    public sealed class SendDeviceCommandRequest
    {
        public DeviceCommandType Command { get; set; }
        public int? Percentage { get; set; }
    }

    // ============================
    // POST command
    // ============================
    [DeviceAuthorize]
    [HttpPost]
    public async Task<IActionResult> SendCommand(
        [FromBody] SendDeviceCommandRequest request,
        CancellationToken ct)
    {
        var device = HttpContext.Items["Device"] as DeviceEntity;
        if (device is null)
            return Unauthorized();

        // Vent validation
        if (request.Command == DeviceCommandType.Vent)
        {
            if (request.Percentage is null or < 1 or > 99)
                return BadRequest("Vent requires percentage 1–99.");
        }

        var command = new DeviceCommandEntity(
            device.Id,
            request.Command,
            request.Command == DeviceCommandType.Vent ? request.Percentage : null
        );

        _db.DeviceCommands.Add(command);
        await _db.SaveChangesAsync(ct);

        // 200 OK with command info
        return Ok(new
        {
            id = command.Id,
            commandType = (int)command.CommandType,
            targetPercentage = command.TargetPercentage
        });
    }

    // ============================
    // GET pending command
    // ============================
    [DeviceAuthorize]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var device = HttpContext.Items["Device"] as DeviceEntity;
        if (device == null)
            return Unauthorized();

        var command = await _mediator.Send(
            new GetPendingDeviceCommandQuery(device.Id),
            ct);

        if (command == null)
            return NoContent(); // 204

        return Ok(new
        {
            id = command.Id,
            commandType = (int)command.CommandType,
            targetPercentage = command.TargetPercentage
        });
    }

    // ============================
    // POST ack
    // ============================
    [DeviceAuthorize]
    [HttpPost("{id:guid}/ack")]
    public async Task<IActionResult> Ack(Guid id, CancellationToken ct)
    {
        var device = HttpContext.Items["Device"] as DeviceEntity;
        if (device == null)
            return Unauthorized();

        var cmd = await _db.DeviceCommands
            .Where(x => x.DeviceId == device.Id && x.Id == id)
            .FirstOrDefaultAsync(ct);

        if (cmd == null)
            return NotFound();

        cmd.MarkAcknowledged();
        await _db.SaveChangesAsync(ct);

        return Ok();
    }
}
