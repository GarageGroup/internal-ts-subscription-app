using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum SubscriptionSetGetFailureCode
{
    Unknown,
    
    [Problem(FailureStatusCode.NotFound, "Bot user was not found")]
    BotUserNotFound
}