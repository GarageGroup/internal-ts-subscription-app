using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

[Endpoint(EndpointMethod.Delete, Func.RouteUnsubscribe, Summary = Func.SummaryUnsubscribe, Description = Func.DescriptionUnsubscribe)]
[EndpointTag(Func.Tag)]
public interface INotificationUnsubscribeFunc
{
    ValueTask<Result<Unit, Failure<NotificationUnsubscribeFailureCode>>> InvokeAsync(
        NotificationUnsubscribeIn input, CancellationToken cancellationToken);
}