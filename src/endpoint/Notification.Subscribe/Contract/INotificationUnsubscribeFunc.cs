using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/notification/unsubscribe", Description = "Unsubscribe bot user from notification")]
[EndpointTag("Notification")]
public interface INotificationUnsubscribeFunc
{
    ValueTask<Result<Unit, Failure<NotificationUnsubscribeFailureCode>>> InvokeAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken);
}