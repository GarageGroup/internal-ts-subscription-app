using System;
using GarageGroup.Internal.Timesheet;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Option;
using PrimeFuncPack;

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