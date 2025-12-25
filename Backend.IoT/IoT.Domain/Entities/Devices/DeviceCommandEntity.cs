namespace IoT.Domain.Entities.Devices;

public class DeviceCommandEntity
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }

    public DeviceCommandType CommandType { get; private set; }
    public int? TargetPercentage { get; private set; }

    public DeviceCommandStatus Status { get; /* private */ set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? AcknowledgedAtUtc { get; private set; }

    private DeviceCommandEntity() { }

    public DeviceCommandEntity(
        Guid deviceId,
        DeviceCommandType commandType,
        int? targetPercentage = null)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        CommandType = commandType;
        TargetPercentage = targetPercentage;

        Status = DeviceCommandStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAcknowledged()
    {
        Status = DeviceCommandStatus.Acknowledged;
        AcknowledgedAtUtc = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == DeviceCommandStatus.Pending)
            Status = DeviceCommandStatus.Cancelled;
    }

}
