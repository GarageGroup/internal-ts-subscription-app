using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;
using System;

namespace GarageGroup.Internal.Timesheet;

internal static partial class Application
{
    private static Dependency<IDataverseApiClient> UseDataverseApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("DataverseApi")
        .UseTokenCredentialStandard()
        .UsePollyStandard()
        .UseDataverseApiClient("Dataverse");

    private static Dependency<ISqlApi> UseSqlApi()
        =>
        DataverseDbProvider.Configure("Dataverse")
        .UseSqlApi();

    private static IConfiguration GetConfiguration(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<IConfiguration>();
    
    private static NotificationSubscribeOption ResolveNotificationSubscribeOption(IServiceProvider serviceProvider)
        =>
        new()
        {
            BotId = serviceProvider.ResolveBotId()
        };

    private static long ResolveBotId(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<IConfiguration>().GetValue<long>("TelegramBot:Id");
}