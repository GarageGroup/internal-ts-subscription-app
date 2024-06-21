using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static ProjectSetSearchMetadata;

public enum ProjectSetSearchFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.Forbidden, FailureCode.ForbiddenMessage)]
    Forbidden
}