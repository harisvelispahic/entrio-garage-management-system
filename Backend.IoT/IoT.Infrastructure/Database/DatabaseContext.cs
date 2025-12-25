using IoT.Domain.Entities.Devices;
using IoT.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IoT.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
    public DbSet<DeviceStatusEntity> DeviceStatuses => Set<DeviceStatusEntity>();
    public DbSet<DeviceCommandEntity> DeviceCommands => Set<DeviceCommandEntity>();
    public DbSet<DeviceEventEntity> DeviceEvents => Set<DeviceEventEntity>();
    public DbSet<ScheduleEntity> Schedules => Set<ScheduleEntity>();
    public DbSet<OwnerAccountEntity> OwnerAccounts => Set<OwnerAccountEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
