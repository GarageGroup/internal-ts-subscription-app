using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum ProjectSetSearchFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.Forbidden, "This method is forbidden for your account")]
    Forbidden
}