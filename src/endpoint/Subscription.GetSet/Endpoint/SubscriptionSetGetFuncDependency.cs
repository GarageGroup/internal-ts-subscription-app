using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class SubscriptionSetGetFuncDependency
{
    public static Dependency<SubscriptionSetGetEndpoint> UseSubscriptionSetGetEndpoint<TDataverseApi, TBotApi>(
        this Dependency<TDataverseApi, TBotApi> dependency)
        where TDataverseApi : IDataverseEntitySetGetSupplier
        where TBotApi : IBotInfoGetSupplier
    {
        return dependency.Fold(CreateFunc).Map(SubscriptionSetGetEndpoint.Resolve);

        static SubscriptionSetGetFunc CreateFunc(TDataverseApi dataverseApi, TBotApi botApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(botApi);
            
            return new SubscriptionSetGetFunc(dataverseApi, botApi);
        }
    }
}