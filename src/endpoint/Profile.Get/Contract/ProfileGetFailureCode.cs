using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static ProfileGetMetadata;

public enum ProfileGetFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, FailureCode.NotFoundMessage)]
    NotFound
}