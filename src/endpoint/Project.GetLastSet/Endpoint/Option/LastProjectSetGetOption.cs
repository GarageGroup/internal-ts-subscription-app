namespace GarageGroup.Internal.Timesheet;

public sealed record class LastProjectSetGetOption
{
    public LastProjectSetGetOption(int daysPeriod)
        =>
        DaysPeriod = daysPeriod;

    public int DaysPeriod { get; }
}