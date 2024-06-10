using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc
{
    public ValueTask<Result<Unit, Failure<NotificationUnsubscribeFailureCode>>> InvokeAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallel(
            FindBotUserIdAsync, 
            FindNotificationTypeIdAsync)
        .MapSuccess(
            @out => NotificationSubscriptionJson.BuildDataverseUpdateInput(
                botUserId: @out.Item1,
                typeId: @out.Item2,
                operationType: DataverseUpdateOperationType.Update,
                subscription: new NotificationSubscriptionJson
                {
                    IsDisabled = true
                }))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.MapFailureCode(MapFailureWhenUpdateSubscription))
        .Recover(
            RecoverUnsubscribeFailure);
    
    private Task<Result<Guid, Failure<NotificationUnsubscribeFailureCode>>> FindBotUserIdAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            input => TelegramBotUserJson.BuildGetInput(input.SystemUserId, option.BotId))
        .PipeValue(
            dataverseApi.GetEntityAsync<TelegramBotUserJson>)
        .Map(
            static response => response.Value.Id,
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingUnsubscribeUser));

    private Task<Result<Guid, Failure<NotificationUnsubscribeFailureCode>>> FindNotificationTypeIdAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            MapToNotificationTypeKey)
        .MapSuccess(
            NotificationTypeJson.BuildGetInput)
        .ForwardValue(
            dataverseApi.GetEntityAsync<NotificationTypeJson>,
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingUnsubscribeNotificationType))
        .MapSuccess(
            static response => response.Value.Id);
}
