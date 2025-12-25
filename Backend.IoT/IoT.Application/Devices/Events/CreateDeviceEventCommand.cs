using IoT.Domain.Entities.Devices;
using MediatR;

namespace IoT.Application.Devices.Events;

public sealed record CreateDeviceEventCommand(
    Guid DeviceId,
    DeviceEventType Type,
    DeviceEventSource Source
) : IRequest;
