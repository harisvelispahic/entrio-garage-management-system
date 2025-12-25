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

        // nothing to do
        if (settings is null || !settings.Enabled)
            return;

        // when should auto-close run
        var executeAt = DateTime.UtcNow.AddSeconds(settings.AfterSeconds);

        // target % (null = full close), will be ignored for close command
        var target = 50;

        var schedule = new ScheduleEntity(
            deviceId: deviceId,
            commandType: DeviceCommandType.Close,
            targetPercentage: target,
            executeAtUtcUtc: executeAt
        );

        _db.Schedules.Add(schedule);
        await _db.SaveChangesAsync(default);
    }
}
