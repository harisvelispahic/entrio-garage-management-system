using IoT.Domain.Entities.Devices;
using MediatR;

namespace IoT.Application.Devices.Commands.GetPending;

public sealed record GetPendingDeviceCommandQuery(
    Guid DeviceId
) : IRequest<DeviceCommandEntity?>;
