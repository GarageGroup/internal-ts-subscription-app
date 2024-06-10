using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<NotificationSubscribeEndpoint> UseNotificationSubscribeEndpoint()
        => 
        UseDataverseApi().With(ResolveNotificationSubscribeOption).UseNotificationSubscribe();

    private static NotificationSubscribeOption ResolveNotificationSubscribeOption(IServiceProvider serviceProvider) 
        => 
        new()
        {
            BotId = serviceProvider.GetRequiredService<IConfiguration>().GetValue<long>("TelegramBotId")
        };
}