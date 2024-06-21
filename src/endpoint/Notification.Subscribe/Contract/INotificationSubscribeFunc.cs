using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

[Endpoint(EndpointMethod.Post, Func.RouteSubscribe, Summary = Func.SummarySubscribe, Description = Func.DescriptionSubscribe)]
[EndpointTag(Func.Tag)]
public interface INotificationSubscribeFunc
{
    ValueTask<Result<Unit, Failure<NotificationSubscribeFailureCode>>> InvokeAsync(
        NotificationSubscribeIn input, CancellationToken cancellationToken);
}