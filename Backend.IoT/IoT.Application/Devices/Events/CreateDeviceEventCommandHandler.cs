using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using MediatR;

namespace IoT.Application.Devices.Events;

public sealed class CreateDeviceEventCommandHandler
    : IRequestHandler<CreateDeviceEventCommand>
{
    private readonly IAppDbContext _db;

    public CreateDeviceEventCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        CreateDeviceEventCommand request,
        CancellationToken cancellationToken)
    {
        var ev = new DeviceEventEntity(
            request.DeviceId,
            request.Type,
            request.Source
        );

        _db.DeviceEvents.Add(ev);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
