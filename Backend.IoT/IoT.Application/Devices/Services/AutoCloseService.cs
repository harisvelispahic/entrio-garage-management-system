using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using Microsoft.EntityFrameworkCore;

public class AutoCloseService
{
    private readonly IAppDbContext _db;

    public AutoCloseService(IAppDbContext db)
    {
        _db = db;
    }

    public async Task ScheduleAutoCloseAsync(Guid deviceId)
    {
        // load settings
        var settings = await _db.AutoCloseSettings
            .FirstOrDefaultAsync(x => x.DeviceId == deviceId);

        if (settings is null || !settings.Enabled)
            return;

        // ❗ 1) deactivate all existing future auto-close schedules
        var existing = await _db.Schedules
            .Where(s =>
                s.DeviceId == deviceId &&
                s.IsActive &&
                !s.WasTriggered)
            .ToListAsync();

        foreach (var s in existing)
            s.Deactivate();

        // ❗ 2) calculate new execution time
        var executeAt = DateTime.UtcNow.AddSeconds(settings.AfterSeconds);

        var schedule = new ScheduleEntity(
            deviceId: deviceId,
            commandType: DeviceCommandType.Close,
            targetPercentage: null,        // full close
            executeAtUtcUtc: executeAt
        );

        _db.Schedules.Add(schedule);

        await _db.SaveChangesAsync(default);
    }

}
