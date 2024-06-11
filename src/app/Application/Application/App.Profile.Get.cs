using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;
using System;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<ProfileGetEndpoint> UseProfileGetEndpoint()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            ResolveProfileGetOption)
        .UseProfileGetEndpoint();

    private static ProfileGetOption ResolveProfileGetOption(IServiceProvider serviceProvider)
        =>
        new()
        {
            BotId = serviceProvider.ResolveBotId()
        };
}