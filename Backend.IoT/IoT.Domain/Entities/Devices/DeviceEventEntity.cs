
namespace IoT.Domain.Entities.Devices;

public class DeviceEventEntity
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }

    public DeviceEventType EventType { get; private set; }
    public DeviceEventSource Source { get; private set; }
    public string? Details { get; private set; }

    public DateTime OccurredAtUtc { get; private set; }

    private DeviceEventEntity() { }

    public DeviceEventEntity(
        Guid deviceId,
        DeviceEventType eventType,
        DeviceEventSource source,
        string? details = null)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        EventType = eventType;
        Source = source;
        Details = details;
        OccurredAtUtc = DateTime.UtcNow;
    }
}
