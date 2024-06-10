namespace GarageGroup.Internal.Timesheet;

internal sealed record class DailyNotificationUserPreferencesJson
{
    public int WorkedHours { get; init; }

    public string? FlowRuntime { get; init; }
}
