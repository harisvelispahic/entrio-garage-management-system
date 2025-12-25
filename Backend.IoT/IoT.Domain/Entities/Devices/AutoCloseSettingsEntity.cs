namespace IoT.Domain.Entities.Devices;

public class AutoCloseSettingsEntity
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }

    public bool Enabled { get; private set; }

    // how long after open to auto-close
    public int AfterSeconds { get; private set; }

    private AutoCloseSettingsEntity() { }

    public AutoCloseSettingsEntity(Guid deviceId, bool enabled, int afterSeconds)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        Enabled = enabled;
        AfterSeconds = afterSeconds;
    }

    public void Update(bool enabled, int afterSeconds)
    {
        Enabled = enabled;
        AfterSeconds = afterSeconds;
    }
}
