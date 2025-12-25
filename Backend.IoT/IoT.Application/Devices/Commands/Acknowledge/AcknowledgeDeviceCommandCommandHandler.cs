using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Devices.Commands.Acknowledge;

public sealed class AcknowledgeDeviceCommandCommandHandler
    : IRequestHandler<AcknowledgeDeviceCommandCommand>
{
    private readonly IAppDbContext _db;

    public AcknowledgeDeviceCommandCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        AcknowledgeDeviceCommandCommand request,
        CancellationToken ct)
    {
        var command = await _db.DeviceCommands
            .FirstOrDefaultAsync(c => c.Id == request.CommandId, ct);

        if (command == null)
            throw new InvalidOperationException("Command not found.");

        if (command.Status != DeviceCommandStatus.Pending)
            return; // idempotent ACK

        command.MarkAcknowledged();

        await _db.SaveChangesAsync(ct);
    }
}
