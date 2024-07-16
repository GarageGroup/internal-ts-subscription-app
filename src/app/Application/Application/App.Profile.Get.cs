using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<ProfileGetEndpoint> UseProfileGetEndpoint()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            UseBotApi())
        .UseProfileGetEndpoint();
}