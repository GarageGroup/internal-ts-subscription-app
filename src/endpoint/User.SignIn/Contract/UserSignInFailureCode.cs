using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum UserSignInFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, "System user not found")]
    SystemUserNotFound
}