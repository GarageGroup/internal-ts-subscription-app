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
            results => new SaveSubscriptionInput
            {
                BotUserId = results.Item1, 
                NotificationTypeId = results.Item2, 
                NotificationPreferences = input.UserPreferenceJson
            })
        .ForwardValue(
            SaveSubscriptionAsync);

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
            failure => failure.MapFailureCode(MapFindBotUserFailureCode));
    
    private static NotificationSubscribeFailureCode MapFindBotUserFailureCode(DataverseFailureCode failureCode) 
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
            input => NotificationTypeJson.BuildGetInput(input.NotificationType))
        .PipeValue(
            dataverseApi.GetEntitySetAsync<NotificationTypeJson>)
        .MapFailure(
            MapFailure)
        .Forward(
            ProcessFindNotificationTypeResponse);

    private static Result<Guid, Failure<NotificationSubscribeFailureCode>> ProcessFindNotificationTypeResponse(DataverseEntitySetGetOut<NotificationTypeJson> response)
    {
        var types = response.Value;
        if (types.Length == 0)
        {
            return Failure.Create(
                NotificationSubscribeFailureCode.NotificationTypeNotFound,
                "Notification type with the specified key was not found");
        }

        return types[0].Id;
    }
    
    private record SaveSubscriptionInput
    {
        public Guid NotificationTypeId { get; init; }
        
        public Guid BotUserId { get; init; }
        
        public JsonElement? NotificationPreferences { get; init; }
    }
    
    private ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> SaveSubscriptionAsync(
        SaveSubscriptionInput input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            FindSubscriptionAsync)
        .ForwardValue(
            async (subscription, cancellationToken) =>
            {
                var subscriptionToSave = new NotificationSubscriptionJson
                {
                    BotUserId = NotificationSubscriptionJson.BuildBotUserLookupValue(input.BotUserId),
                    NotificationType = NotificationSubscriptionJson.BuildNotificationTypeLookupValue(input.NotificationTypeId),
                    IsDisabled = false,
                    NotificationPreferences = input.NotificationPreferences?.ToString() ?? subscription?.NotificationPreferences
                };
                
                if (subscription is null)
                {
                    return await CreateSubscriptionAsync(subscriptionToSave, cancellationToken);
                }

                return await UpdateSubscriptionAsync(subscription.Id, subscriptionToSave, cancellationToken);
            });
    
    private ValueTask<Result<NotificationSubscriptionGetJson?, Failure<NotificationSubscribeFailureCode>>> FindSubscriptionAsync(SaveSubscriptionInput input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            input => NotificationSubscriptionGetJson.BuildDataverseGetInput(input.BotUserId, input.NotificationTypeId))
        .PipeValue(
            dataverseApi.GetEntitySetAsync<NotificationSubscriptionGetJson>)
        .Map(
            static response => response.Value.Length == 1 ? response.Value[0] : null,
            MapFailure);
    
    private ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> CreateSubscriptionAsync(NotificationSubscriptionJson input, CancellationToken cancellationToken) 
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            NotificationSubscriptionJson.BuildDataverseCreateInput)
        .PipeValue(
            dataverseApi.CreateEntityAsync)
        .MapFailure(
            MapFailure);
    
    private ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> UpdateSubscriptionAsync(Guid subscriptionId, NotificationSubscriptionJson subscription, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            subscription, cancellationToken)
        .Pipe(
            input => NotificationSubscriptionJson.BuildDataverseUpdateInput(subscriptionId, input))
        .PipeValue(
            dataverseApi.UpdateEntityAsync)
        .MapFailure(
            MapFailure);

    private static Failure<NotificationSubscribeFailureCode> MapFailure(Failure<DataverseFailureCode> failure)
        => 
        failure.MapFailureCode(MapFailureCode);
    
    private static NotificationSubscribeFailureCode MapFailureCode(DataverseFailureCode failureCode)
        => 
        failureCode switch
        { 
            _ => NotificationSubscribeFailureCode.Unknown 
        };
}