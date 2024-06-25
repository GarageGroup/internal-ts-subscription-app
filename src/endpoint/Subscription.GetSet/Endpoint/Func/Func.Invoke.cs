using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Internal.Timesheet;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Inner.Json;

namespace GarageGroup.Internal.Timesheet;

partial class SubscriptionSetGetFunc
{
    public ValueTask<Result<SubscriptionSetGetOut, Failure<SubscriptionSetGetFailureCode>>> InvokeAsync(
        SubscriptionSetGetIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            FindBotUserIdAsync)
        .MapSuccess(
            SubscriptionJson.BuildGetInput)
        .ForwardValue(
            dataverseApi.GetEntitySetAsync<SubscriptionJson>,
            failure => failure.WithFailureCode(SubscriptionSetGetFailureCode.Unknown))
        .MapSuccess(
            response => new SubscriptionSetGetOut
            {
                Subscriptions = response.Value.Map(MapToSubscriptionDto).Map(MapToSubscription)
            });
    
    private ValueTask<Result<Guid, Failure<SubscriptionSetGetFailureCode>>> FindBotUserIdAsync(
        SubscriptionSetGetIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            input => TelegramBotUserJson.BuildGetInput(input.SystemUserId, option.BotId))
        .PipeValue(
            dataverseApi.GetEntityAsync<TelegramBotUserJson>)
        .Map(
            static response => response.Value.Id,
            static failure => failure.MapFailureCode(MapFailureCodeWhenSearchForUser));
    
}