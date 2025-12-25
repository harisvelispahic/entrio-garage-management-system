using IoT.Domain.Entities.Devices;

namespace IoT.Application.Devices;

public interface IDeviceAuthenticator
{
    Task<DeviceEntity?> AuthenticateAsync(string deviceKey);
}
