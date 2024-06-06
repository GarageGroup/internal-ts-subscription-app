using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum TimesheetUpdateFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, "Timesheet not found")]
    NotFound
}