using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Option;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class SubscriptionSetGetFuncDependency
{
    public static Dependency<SubscriptionSetGetEndpoint> UseSubscriptionSetGetEndpoint(
        this Dependency<IDataverseApiClient, SubscriptionSetGetOption> dependency)
    {
        return dependency.Fold(CreateFunc).Map(SubscriptionSetGetEndpoint.Resolve);

        SubscriptionSetGetFunc CreateFunc(IDataverseApiClient dataverseApi, SubscriptionSetGetOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);
            
            return new SubscriptionSetGetFunc(dataverseApi, option);
        }
    }
}