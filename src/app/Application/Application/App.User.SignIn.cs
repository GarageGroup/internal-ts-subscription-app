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
        =>
        new()
        {
            BotId = serviceProvider.ResolveBotId(),
            BotName = (serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("TelegramBot:Name")).OrEmpty(),
            BotToken = (serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("TelegramBot:Token")).OrEmpty()
        };
}