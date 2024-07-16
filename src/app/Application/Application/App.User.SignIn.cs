using GarageGroup.Infra;
using PrimeFuncPack;
using System;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<UserSignInEndpoint> UseUserSignInEndpoint()
        =>
        Pipeline.Pipe(
            UseDataverseApi())
        .With(
            UseBotApi())
        .With(
            ResolveUserSignInOption)
        .UseUserSignInEndpoint();

    private static UserSignInOption ResolveUserSignInOption(IServiceProvider serviceProvider)
        =>
        new(
            botToken: serviceProvider.GetConfiguration().GetBotTokenOrThrow());
}