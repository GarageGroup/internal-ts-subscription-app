using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<ProfileUpdateEndpoint> UseProfileUpdateEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            UseBotApi())
        .UseProfileUpdateEndpoint();
}