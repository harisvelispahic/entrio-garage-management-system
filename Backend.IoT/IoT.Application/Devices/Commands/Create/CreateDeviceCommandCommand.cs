using IoT.Domain.Entities.Devices;
using MediatR;

namespace IoT.Application.Devices.Commands.Create;

public sealed record CreateDeviceCommandCommand(
    Guid DeviceId,
    DeviceCommandType CommandType,
    int? Percentage
) : IRequest;
