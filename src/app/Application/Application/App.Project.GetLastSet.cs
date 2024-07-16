using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<LastProjectSetGetEndpoint> UseLastProjectSetGetEndpoint()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            ResolveLastProjectSetGetOption)
        .UseLastProjectSetGetEndpoint();

    private static LastProjectSetGetOption ResolveLastProjectSetGetOption(IServiceProvider serviceProvider)
        =>
        new(serviceProvider.GetConfiguration().GetValue("Project:LastDaysPeriod", 30));
}