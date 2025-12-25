
namespace IoT.Domain.Entities.Devices;

public class DeviceStatusEntity
{
    public Guid DeviceId { get; private set; }

    public DoorState DoorState { get; private set; }
    public int PositionPercent { get; private set; }
    public bool ObstacleDetected { get; private set; }

    public DateTime? OpenedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private DeviceStatusEntity() { }

    public DeviceStatusEntity(Guid deviceId)
    {
        DeviceId = deviceId;
        DoorState = DoorState.Closed;
        PositionPercent = 0;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Update(DoorState state, int positionPercent, bool obstacleDetected)
    {
        DoorState = state;
        PositionPercent = positionPercent;
        ObstacleDetected = obstacleDetected;

        if (state == DoorState.Open && OpenedAtUtc == null)
            OpenedAtUtc = DateTime.UtcNow;

        if (state == DoorState.Closed)
            OpenedAtUtc = null;

        UpdatedAtUtc = DateTime.UtcNow;
    }
}
