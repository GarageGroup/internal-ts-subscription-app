using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum TimesheetUpdateFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, "Timesheet not found")]
    NotFound,

    [Problem(FailureStatusCode.BadRequest, "An unexpected project type")]
    UnexpectedProjectType,

    [Problem(FailureStatusCode.NotFound, "Project not found")]
    ProjectNotFound
}