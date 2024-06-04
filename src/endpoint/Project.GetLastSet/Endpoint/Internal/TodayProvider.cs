using System;

namespace GarageGroup.Internal.Timesheet;

public sealed class TodayProvider : ITodayProvider
{
    public static readonly TodayProvider Instance;

    static TodayProvider()
        =>
        Instance = new();

    private TodayProvider()
    {
    }

    public DateOnly Today
        =>
        DateOnly.FromDateTime(DateTime.Now);
}
