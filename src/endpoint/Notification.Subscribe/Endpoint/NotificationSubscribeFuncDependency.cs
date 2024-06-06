using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class NotificationSubscribeFuncDependency
{
    public static Dependency<NotificationSubscribeEndpoint> UseNotificationSubscribe(this Dependency<IDataverseApiClient> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(NotificationSubscribeEndpoint.Resolve);
    
        static NotificationSubscribeFunc CreateFunc(IDataverseApiClient dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}