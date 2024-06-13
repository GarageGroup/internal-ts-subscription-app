using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum ProfileUpdateFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, "Profile not found")]
    NotFound,
}