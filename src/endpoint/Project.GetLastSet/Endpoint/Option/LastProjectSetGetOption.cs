namespace GarageGroup.Internal.Timesheet;

public sealed record class LastProjectSetGetOption
{
    public LastProjectSetGetOption(int lastDaysPeriod)
        =>
        LastDaysPeriod = lastDaysPeriod;

    public int LastDaysPeriod { get; }
}