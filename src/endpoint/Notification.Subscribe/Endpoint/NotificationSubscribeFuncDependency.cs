using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class NotificationSubscribeFuncDependency
{
    public static Dependency<NotificationSubscribeEndpoint> UseNotificationSubscribe<TBotApi>(
        this Dependency<IDataverseApiClient, TBotApi> dependency)
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(NotificationSubscribeEndpoint.Resolve);
    }
    
    public static Dependency<NotificationUnsubscribeEndpoint> UseNotificationUnsubscribe<TBotApi>(
        this Dependency<IDataverseApiClient, TBotApi> dependency)
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(NotificationUnsubscribeEndpoint.Resolve);
    }
    
    private static NotificationSubscribeFunc CreateFunc<TBotApi>(IDataverseApiClient dataverseApi, TBotApi botApi)
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dataverseApi);
        ArgumentNullException.ThrowIfNull(botApi);
            
        return new(dataverseApi, botApi);
    }
}