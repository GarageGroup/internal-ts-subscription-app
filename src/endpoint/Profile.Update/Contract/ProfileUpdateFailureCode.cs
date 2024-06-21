using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static ProfileUpdateMetadata;

public enum ProfileUpdateFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, FailureCode.NotFoundMessage)]
    NotFound
}