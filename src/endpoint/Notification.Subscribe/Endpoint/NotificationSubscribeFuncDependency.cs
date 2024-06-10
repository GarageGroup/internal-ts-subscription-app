using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class NotificationSubscribeFuncDependency
{
    public static Dependency<NotificationSubscribeEndpoint> UseNotificationSubscribe(
        this Dependency<IDataverseApiClient, NotificationSubscribeOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(NotificationSubscribeEndpoint.Resolve);
    }
    
    public static Dependency<NotificationUnsubscribeEndpoint> UseNotificationUnsubscribe(
        this Dependency<IDataverseApiClient, NotificationSubscribeOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(NotificationUnsubscribeEndpoint.Resolve);
    }
    
    private static NotificationSubscribeFunc CreateFunc(IDataverseApiClient dataverseApi, NotificationSubscribeOption option)
    {
        ArgumentNullException.ThrowIfNull(dataverseApi);
        ArgumentNullException.ThrowIfNull(option);
            
        return new(dataverseApi, option);
    }
}