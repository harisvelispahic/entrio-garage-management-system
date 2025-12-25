using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IoT.Infrastructure.Background;

public class ScheduleWorker : BackgroundService
{
    private readonly IServiceProvider _provider;

    public ScheduleWorker(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var nowUtc = DateTime.UtcNow;

            // 1) find all active & not yet triggered schedules that are due
            var dueSchedules = await db.Schedules
                .Where(s => s.IsActive && !s.WasTriggered && s.ExecuteAtUtc <= nowUtc)
                .ToListAsync(stoppingToken);

            foreach (var schedule in dueSchedules)
            {
                // 2) Create the corresponding DeviceCommand
                var command = new DeviceCommandEntity(
                    schedule.DeviceId,
                    schedule.CommandType,
                    schedule.CommandType == DeviceCommandType.Vent
                        ? schedule.TargetPercentage
                        : null,
                    suppressAutoClose: true   // IMPORTANT
                );


                db.DeviceCommands.Add(command);

                // 3) mark schedule as triggered (so it doesn't fire again)
                schedule.MarkTriggered();
            }

            await db.SaveChangesAsync(stoppingToken);

            // Poll every 5 seconds (you can tune this)
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
