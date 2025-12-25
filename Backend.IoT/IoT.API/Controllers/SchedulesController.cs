using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using Microsoft.AspNetCore.Mvc;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly IAppDbContext _db;

    public SchedulesController(IAppDbContext db)
    {
        _db = db;
    }

    public sealed class CreateScheduleRequest
    {
        public Guid DeviceId { get; set; }
        public DeviceCommandType CommandType { get; set; }
        public int? TargetPercentage { get; set; }

        /// <summary>
        /// When to execute, in UTC (e.g. 2025-12-25T20:30:00Z).
        /// </summary>
        public DateTime ExecuteAtUtc { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateScheduleRequest request,
        CancellationToken ct)
    {
        // Basic validation
        if (request.CommandType == DeviceCommandType.Vent)
        {
            if (request.TargetPercentage is null or < 1 or > 99)
                return BadRequest("Vent requires targetPercentage between 1 and 99.");
        }

        // For Open/Close/Stop we ignore TargetPercentage
        int? effectiveTarget = request.CommandType == DeviceCommandType.Vent
            ? request.TargetPercentage
            : null;

        var schedule = new ScheduleEntity(
            request.DeviceId,
            request.CommandType,
            effectiveTarget,
            request.ExecuteAtUtc.ToUniversalTime()
        );

        _db.Schedules.Add(schedule);
        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            id = schedule.Id,
            deviceId = schedule.DeviceId,
            commandType = (int)schedule.CommandType,
            targetPercentage = schedule.TargetPercentage,
            executeAtUtc = schedule.ExecuteAtUtc
        });
    }
}
