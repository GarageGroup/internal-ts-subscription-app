using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum NotificationSubscribeFailureCode
{
    Unknown,
    
    [Problem(FailureStatusCode.BadRequest, true)]
    BotUserNotFound,

    [Problem(FailureStatusCode.BadRequest, true)]
    NotificationTypeNotFound,
    
    [Problem(FailureStatusCode.BadRequest, true)]
    InvalidQuery,
}