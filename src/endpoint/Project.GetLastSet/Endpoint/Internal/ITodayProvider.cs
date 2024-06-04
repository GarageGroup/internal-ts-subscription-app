using System;

namespace GarageGroup.Internal.Timesheet;

public interface ITodayProvider
{
    DateOnly Today { get; }
}