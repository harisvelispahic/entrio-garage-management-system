using IoT.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IAppDbContext _db;

    public EventsController(IAppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents(CancellationToken ct)
    {
        var events = await _db.DeviceEvents
            .OrderByDescending(e => e.OccurredAtUtc)
            .Take(200) // avoid dumping database
            .Select(e => new
            {
                id = e.Id,
                eventType = e.EventType.ToString(),
                source = e.Source.ToString(),
                timestamp = e.OccurredAtUtc,
                details = e.Details
            })
            .ToListAsync(ct);

        return Ok(events);
    }
}
