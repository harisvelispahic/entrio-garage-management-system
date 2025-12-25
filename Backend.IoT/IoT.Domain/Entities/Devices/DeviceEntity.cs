using IoT.Domain.Entities.Devices;

namespace IoT.Domain.Entities.Devices;

public class DeviceEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string DeviceKeyHash { get; private set; } = null!;
    public string DeviceKeySalt { get; private set; } = null!;

    public DateTime LastSeenAtUtc { get; private set; }



    // Navigation
    public DeviceStatusEntity Status { get; private set; } = null!;
    public ICollection<DeviceCommandEntity> Commands { get; private set; } = new List<DeviceCommandEntity>();
    public ICollection<DeviceEventEntity> Events { get; private set; } = new List<DeviceEventEntity>();
    public ICollection<ScheduleEntity> Schedules { get; private set; } = new List<ScheduleEntity>();

    private DeviceEntity() { }

    public DeviceEntity(string name, string deviceKeyHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        DeviceKeyHash = deviceKeyHash;
        LastSeenAtUtc = DateTime.UtcNow;
    }

    public void UpdateLastSeen()
    {
        LastSeenAtUtc = DateTime.UtcNow;
    }
}