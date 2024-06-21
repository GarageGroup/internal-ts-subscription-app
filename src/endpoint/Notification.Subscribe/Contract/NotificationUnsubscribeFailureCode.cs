using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

public enum NotificationUnsubscribeFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, true)]
    InvalidQuery,

    [Problem(FailureStatusCode.BadRequest, FailureCode.NotificationTypeInvalidMessage)]
    NotificationTypeInvalid,

    [Problem(FailureStatusCode.NotFound, FailureCode.NotificationTypeNotFoundMessage)]
    NotificationTypeNotFound,

    [Problem(FailureStatusCode.NotFound, FailureCode.BotUserNotFoundMessage)]
    BotUserNotFound
}