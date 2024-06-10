using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc
{
    public ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InvokeAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallel(
            FindBotUserIdAsync, 
            FindNotificationTypeIdAsync)
        .MapSuccess(
            @out => NotificationSubscriptionJson.BuildDataverseUpsertInput(
                botUserId: @out.Item1,
                typeId: @out.Item2,
                subscription: new NotificationSubscriptionJson
                {
                    IsDisabled = true
                }))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.WithFailureCode(NotificationSubscribeFailureCode.Unknown))
        .Recover(failure => Unit.Value);

    private Task<Result<Guid, Failure<NotificationSubscribeFailureCode>>> FindBotUserIdAsync(
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
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingBotUser));

    private Task<Result<Guid, Failure<NotificationSubscribeFailureCode>>> FindNotificationTypeIdAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            input => MapToNotificationTypeKey(input.NotificationType))
        .MapSuccess(
            NotificationTypeJson.BuildGetInput)
        .ForwardValue(
            dataverseApi.GetEntityAsync<NotificationTypeJson>,
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingNotificationType))
        .MapSuccess(
            static response => response.Value.Id);
}
