using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class SubscriptionSetGetFuncDependency
{
    public static Dependency<SubscriptionSetGetEndpoint> UseSubscriptionSetGetEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi, SubscriptionSetGetOption> dependency)
        where TDataverseApi : IDataverseEntitySetGetSupplier
    {
        return dependency.Fold(CreateFunc).Map(SubscriptionSetGetEndpoint.Resolve);

        static SubscriptionSetGetFunc CreateFunc(TDataverseApi dataverseApi, SubscriptionSetGetOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);
            
            return new SubscriptionSetGetFunc(dataverseApi, option);
        }
    }
}