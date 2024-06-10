using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

public static class NotificationUnsubscribeFuncDependency
{
    public static Dependency<NotificationUnsubscribeEndpoint> UseNotificationUnsubscribe(
        this Dependency<IDataverseApiClient, NotificationSubscribeOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(NotificationUnsubscribeEndpoint.Resolve);
    
        static NotificationSubscribeFunc CreateFunc(IDataverseApiClient dataverseApi, NotificationSubscribeOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);
            
            return new(dataverseApi, option);
        }
    }
}