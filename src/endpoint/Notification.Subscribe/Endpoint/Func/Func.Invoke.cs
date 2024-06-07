using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc
{
    public ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InvokeAsync(NotificationSubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateAndMapToJsonDto)
        .MapSuccess(
            json => new NotificationData(input, json))
        .Forward(
            InnerInvokeAsync);
    
    private Task<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InnerInvokeAsync(NotificationData input, CancellationToken cancellationToken) 
        => 
        AsyncPipeline.Pipe(
            input.Input, cancellationToken)
        .PipeParallel(
            FindBotUserIdAsync,
            FindNotificationTypeIdAsync)
        .MapSuccess(
            results => NotificationSubscriptionJson.BuildDataverseUpsertInput(
                botUserId: results.Item1,
                typeId: results.Item2,
                subscription: input.Subscription))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.WithFailureCode(NotificationSubscribeFailureCode.Unknown));
    
    private Task<Result<Guid, Failure<NotificationSubscribeFailureCode>>> FindBotUserIdAsync(NotificationSubscribeIn input, CancellationToken cancellationToken)
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
        NotificationSubscribeIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            MapToNotificationTypeKey)
        .MapSuccess(
            NotificationTypeJson.BuildGetInput)
        .ForwardValue(
            dataverseApi.GetEntityAsync<NotificationTypeJson>,
            static failure => failure.MapFailureCode(MapFailureCodeWhenFindingNotificationType))
        .MapSuccess(
            static response => response.Value.Id);
}
