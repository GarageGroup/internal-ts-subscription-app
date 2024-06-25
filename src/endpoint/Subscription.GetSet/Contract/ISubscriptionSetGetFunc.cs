using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Get, "getSubscriptions")]
[EndpointTag("Notification")]
public interface ISubscriptionSetGetFunc
{
    ValueTask<Result<SubscriptionSetGetOut, Failure<SubscriptionSetGetFailureCode>>> InvokeAsync(SubscriptionSetGetIn input,
        CancellationToken cancellationToken);
}