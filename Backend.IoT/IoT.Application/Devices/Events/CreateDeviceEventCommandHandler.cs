using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using MediatR;

namespace IoT.Application.Devices.Events;

public sealed class CreateDeviceEventCommandHandler
    : IRequestHandler<CreateDeviceEventCommand>
{
    private readonly IAppDbContext _db;
    private readonly AutoCloseService _autoClose;

    public CreateDeviceEventCommandHandler(
        IAppDbContext db,
        AutoCloseService autoClose)
    {
        _db = db;
        _autoClose = autoClose;
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

        // 👇 ONLY schedule when the door actually opened
        if (request.Type == DeviceEventType.DoorOpened)
        {
            await _autoClose.ScheduleAutoCloseAsync(request.DeviceId);
        }
    }
}
