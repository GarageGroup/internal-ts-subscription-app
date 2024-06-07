namespace GarageGroup.Internal.Timesheet;

public sealed record class NotificationSubscribeOption
{
    public required long BotId { get; init; }
}