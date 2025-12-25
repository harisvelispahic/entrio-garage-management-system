namespace IoT.Domain.Entities.Devices;

public enum DoorState
{
    Closed = 0,
    Opening = 1,
    Open = 2,
    Closing = 3,
    Stopped = 4,
    Error = 5
}

public enum DeviceCommandType
{
    Open = 0,
    Close = 1,
    Stop = 2,
    Vent = 3
}

public enum DeviceCommandStatus
{
    Pending = 0,
    Sent = 1,
    Acknowledged = 2,
    Failed = 3,
    Cancelled = 4
}

public enum DeviceEventType
{
    DoorOpened = 0,
    DoorClosed = 1,
    ObstacleDetected = 2,
    ObstacleCleared = 3,
    AutoCloseTriggered = 4,
    ScheduleTriggered = 5,
    ManualOpen = 6,
    ManualClose = 7
}

public enum DeviceEventSource
{
    Remote = 0,
    LocalRfid = 1,
    Schedule = 2,
    AutoClose = 3,
    System = 4
}
