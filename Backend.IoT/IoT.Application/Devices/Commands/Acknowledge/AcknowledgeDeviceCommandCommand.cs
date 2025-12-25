using MediatR;

namespace IoT.Application.Devices.Commands.Acknowledge;

public sealed record AcknowledgeDeviceCommandCommand(
    Guid CommandId
) : IRequest;
