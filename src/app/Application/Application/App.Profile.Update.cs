using GarageGroup.Infra;
using PrimeFuncPack;
using System;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<ProfileUpdateEndpoint> UseProfileUpdateEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            ResolveProfileUpdateOption)
        .UseProfileUpdateEndpoint();

    private static ProfileUpdateOption ResolveProfileUpdateOption(IServiceProvider serviceProvider)
        =>
        new()
        {
            BotId = serviceProvider.ResolveBotId()
        };
}