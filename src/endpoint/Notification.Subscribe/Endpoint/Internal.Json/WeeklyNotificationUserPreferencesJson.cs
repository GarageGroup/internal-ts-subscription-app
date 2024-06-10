namespace GarageGroup.Internal.Timesheet;

internal sealed record class WeeklyNotificationUserPreferencesJson
{
    public string? Weekday { get; init; }

    public int WorkedHours { get; init; }

    public string? FlowRuntime { get; init; }
}