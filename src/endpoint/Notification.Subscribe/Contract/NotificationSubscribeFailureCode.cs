using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public enum NotificationSubscribeFailureCode
{
    Unknown,
    
    [Problem(FailureStatusCode.NotFound, true)]
    BotUserNotFound,

    [Problem(FailureStatusCode.NotFound, true)]
    NotificationTypeNotFound,
    
    [Problem(FailureStatusCode.BadRequest, true)]
    NotificationTypeInvalid,
    
    [Problem(FailureStatusCode.BadRequest, true)]
    InvalidQuery,
}