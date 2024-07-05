using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            ResolveUserSignInOption)
        .UseUserSignInEndpoint();

    private static UserSignInOption ResolveUserSignInOption(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        return new(
            botId: configuration.GetBotId(),
            botName: configuration["TelegramBot:Name"],
            botToken: configuration["TelegramBot:Token"].OrEmpty());
    }
}