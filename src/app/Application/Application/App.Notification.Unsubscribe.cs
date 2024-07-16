using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<NotificationUnsubscribeEndpoint> UseNotificationUnsubscribeEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            UseBotApi())
        .UseNotificationUnsubscribe();
}