using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum TimesheetCreateFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.Forbidden, "This method is forbidden for your account")]
    Forbidden,

    [Problem(FailureStatusCode.BadRequest, "An unexpected project type")]
    UnexpectedProjectType,

    [Problem(FailureStatusCode.NotFound, "Project not found")]
    ProjectNotFound
}