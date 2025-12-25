using IoT.Application.Common;
using IoT.Application.Devices.Commands.GetPending;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Devices.Commands.GetPending;

public sealed class GetPendingDeviceCommandQueryHandler
    : IRequestHandler<GetPendingDeviceCommandQuery, DeviceCommandEntity?>
{
    private readonly IAppDbContext _db;

    public GetPendingDeviceCommandQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<DeviceCommandEntity?> Handle(
        GetPendingDeviceCommandQuery request,
        CancellationToken ct)
    {
        return await _db.DeviceCommands
            .Where(x => x.DeviceId == request.DeviceId)
            .Where(x => x.Status == DeviceCommandStatus.Pending)
            .OrderBy(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);
    }
}
