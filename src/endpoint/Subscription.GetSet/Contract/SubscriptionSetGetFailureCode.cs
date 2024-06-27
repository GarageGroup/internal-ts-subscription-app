using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public enum SubscriptionSetGetFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.NotFound, FailureCode.BotUserNotFoundMessage)]
    BotUserNotFound
}