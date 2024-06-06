using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc
{
    public ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InvokeAsync(
        NotificationSubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallel(
            FindBotUserIdAsync,
            FindNotificationTypeIdAsync)
        .MapSuccess(
            results =>
            {
                var subscription = new NotificationSubscriptionJson
                {
                    IsDisabled = false,
                    NotificationPreferences = SerializeUserPreferences(input.SubscriptionData)
                };

                return NotificationSubscriptionJson.BuildDataverseUpsertInput(
                    botUserId: results.Item1,
                    typeId: results.Item2,
                    subscription: subscription);
            })
        .ForwardValue(
            UpdateSubscriptionAsync);

    private Task<Result<Guid, Failure<NotificationSubscribeFailureCode>>> FindBotUserIdAsync(NotificationSubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            input => TelegramBotUserJson.BuildGetInput(input.BotId, input.ChatId))
        .PipeValue(
            dataverseApi.GetEntityAsync<TelegramBotUserJson>)
        .Map(
            response => response.Value.Id,
            failure => failure.MapFailureCode(MapFailureCodeWhenFindingBotUser));
    
    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingBotUser(DataverseFailureCode failureCode) 
        => 
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.BotUserNotFound,
            _ => NotificationSubscribeFailureCode.Unknown 
        };

    private Task<Result<Guid, Failure<NotificationSubscribeFailureCode>>> FindNotificationTypeIdAsync(
        NotificationSubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            MapToNotificationTypeKey)
        .Pipe(
            NotificationTypeJson.BuildGetInput)
        .PipeValue(
            dataverseApi.GetEntityAsync<NotificationTypeJson>)
        .Map(
            static response => response.Value.Id,
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingNotificationType));

    private static string MapToNotificationTypeKey(NotificationSubscribeIn input) 
        => 
        input.SubscriptionData switch
        {
            DailySubscriptionData => "dailyTimesheetNotification",
            WeeklySubscriptionData => "weeklyTimesheetNotification",
            _ => throw new NotSupportedException("Not supported type of subscription data")
        };
    
    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingNotificationType(DataverseFailureCode code)
        => 
        code switch
        { 
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.NotificationTypeNotFound, 
            _ => NotificationSubscribeFailureCode.Unknown
        };

    private static string SerializeUserPreferences(BaseSubscriptionData subscriptionData)
    {
        if (subscriptionData is DailySubscriptionData dailySubscriptionData)
        {
            return dailySubscriptionData.UserPreference is not null
                ? JsonSerializer.Serialize(dailySubscriptionData.UserPreference)
                : string.Empty;
        }

        if (subscriptionData is WeeklySubscriptionData weeklySubscriptionData)
        {
            return weeklySubscriptionData.UserPreference is not null
                ? JsonSerializer.Serialize(weeklySubscriptionData.UserPreference)
                : string.Empty;
        }

        throw new NotSupportedException("Not supported type of subscription data");
    }
    
    private ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> UpdateSubscriptionAsync(DataverseEntityUpdateIn<NotificationSubscriptionJson> input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            dataverseApi.UpdateEntityAsync)
        .MapFailure(
            failure => failure.MapFailureCode(code => NotificationSubscribeFailureCode.Unknown));
}