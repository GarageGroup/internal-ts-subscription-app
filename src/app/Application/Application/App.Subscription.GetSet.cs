using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<SubscriptionSetGetEndpoint> UseSubscriptionSetGetEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            ResolveSubscriptionSetGetOption)
        .UseSubscriptionSetGetEndpoint();

    private static SubscriptionSetGetOption ResolveSubscriptionSetGetOption(IServiceProvider serviceProvider)
        =>
        new()
        {
            BotId = serviceProvider.ResolveBotId()
        };
}