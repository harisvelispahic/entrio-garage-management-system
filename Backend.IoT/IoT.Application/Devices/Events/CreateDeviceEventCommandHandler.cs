using IoT.Application.Common;
using IoT.Domain.Entities.Devices;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        // ONLY schedule when the door actually opened
        if (request.Type == DeviceEventType.DoorOpened)
        {
            // get the last executed command for this device
            var lastCommand = await _db.DeviceCommands
                .Where(c => c.DeviceId == request.DeviceId)
                .OrderByDescending(c => c.CreatedAtUtc)
                .FirstOrDefaultAsync(cancellationToken);

            // if there's no command, or auto-close is NOT suppressed → schedule

            //if (lastCommand is null || !lastCommand.SuppressAutoClose)
            //{
            //    await _autoClose.ScheduleAutoCloseAsync(request.DeviceId);
            //}

            if (lastCommand is null
                || !lastCommand.SuppressAutoClose
                || lastCommand.CommandType == DeviceCommandType.Close)
            {
                await _autoClose.ScheduleAutoCloseAsync(request.DeviceId);
            }

        }

    }
}
