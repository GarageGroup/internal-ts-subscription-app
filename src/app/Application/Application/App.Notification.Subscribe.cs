using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<NotificationSubscribeEndpoint> UseNotificationSubscribeEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            UseBotApi())
        .UseNotificationSubscribe();
}