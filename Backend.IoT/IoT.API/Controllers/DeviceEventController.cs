using IoT.API.Security;
using IoT.Application.Devices.Events;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/device/events")]
public class DeviceEventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeviceEventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ============================
    // REQUEST DTO
    // ============================
    public sealed class DeviceEventRequest
    {
        public string Type { get; init; } = null!;
        public string? Source { get; init; }
    }

    // ============================
    // POST /api/device/events
    // ============================
    [DeviceAuthorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromHeader(Name = "X-Device-Key")] string deviceKey,
        [FromBody] DeviceEventRequest request)
    {
        // Device resolved by DeviceAuthorize filter
        var device = HttpContext.Items["Device"] as DeviceEntity;
        if (device is null)
            return Unauthorized();

        // ----------------------------
        // Parse Event Type (REQUIRED)
        // ----------------------------
        if (!Enum.TryParse<DeviceEventType>(
                request.Type,
                ignoreCase: true,
                out var eventType))
        {
            return BadRequest($"Invalid event type: {request.Type}");
        }

        // ----------------------------
        // Parse Event Source (OPTIONAL)
        // Default = System
        // ----------------------------
        DeviceEventSource eventSource;

        if (string.IsNullOrWhiteSpace(request.Source))
        {
            eventSource = DeviceEventSource.System;
        }
        else if (!Enum.TryParse<DeviceEventSource>(
                     request.Source,
                     ignoreCase: true,
                     out eventSource))
        {
            return BadRequest($"Invalid event source: {request.Source}");
        }

        // ----------------------------
        // Send Command
        // ----------------------------
        await _mediator.Send(new CreateDeviceEventCommand(
            device.Id,
            eventType,
            eventSource
        ));

        return Ok();
    }
}
