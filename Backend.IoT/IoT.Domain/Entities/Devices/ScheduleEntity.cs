
namespace IoT.Domain.Entities.Devices;

public class ScheduleEntity
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }

    public DeviceCommandType Action { get; private set; }
    public TimeSpan TimeOfDay { get; private set; }
    public int DaysOfWeekMask { get; private set; } // Mon–Sun bitmask

    public bool IsActive { get; private set; }

    private ScheduleEntity() { }

    public ScheduleEntity(
        Guid deviceId,
        DeviceCommandType action,
        TimeSpan timeOfDay,
        int daysOfWeekMask)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        Action = action;
        TimeOfDay = timeOfDay;
        DaysOfWeekMask = daysOfWeekMask;
        IsActive = true;
    }

    public void Disable()
    {
        IsActive = false;
    }
}
