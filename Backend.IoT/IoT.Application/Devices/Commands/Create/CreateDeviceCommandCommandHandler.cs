using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Devices.Commands.Create;

public sealed class CreateDeviceCommandCommandHandler
    : IRequestHandler<CreateDeviceCommandCommand>
{
    private readonly IAppDbContext _db;

    public CreateDeviceCommandCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
    CreateDeviceCommandCommand request,
    CancellationToken ct)
    {
        var pending = await _db.DeviceCommands
            .Where(c =>
                c.DeviceId == request.DeviceId &&
                c.Status == DeviceCommandStatus.Pending)
            .ToListAsync(ct);

        foreach (var cmd in pending)
            cmd.Status = DeviceCommandStatus.Cancelled;

        var newCommand = new DeviceCommandEntity(
            request.DeviceId,
            request.CommandType,
            request.Percentage
        );

        _db.DeviceCommands.Add(newCommand);

        await _db.SaveChangesAsync(ct);
    }

}
