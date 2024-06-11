using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum ProfileGetFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, "Profile not found")]
    NotFound
}