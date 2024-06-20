using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/subscribeToNotification", Summary = "Subscribe bot user to notification")]
[EndpointTag("Notification")]
public interface INotificationSubscribeFunc
{
    ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InvokeAsync(
        NotificationSubscribeIn input, CancellationToken cancellationToken);
}