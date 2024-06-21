using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<TagSetGetEndpoint> UseTagSetGetEndpoint()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            ResolveTagSetGetOption)
        .UseTagGetSetEndpoint();

    private static TagSetGetOption ResolveTagSetGetOption(IServiceProvider serviceProvider)
        =>
        new(serviceProvider.GetConfiguration().GetValue("Project:TagsDaysPeriod", 30));
}