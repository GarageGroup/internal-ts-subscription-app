using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra.Telegram.Bot;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Service.BotApi.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Internal.Timesheet;

public static class BotApiDependency
{
    public static Dependency<IBotApi> UseBotApi<TTelegramApi>(this Dependency<TTelegramApi> dependency)
        where TTelegramApi : IBotUserApiSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IBotApi>(CreateApi);

        static BotApiImpl CreateApi(TTelegramApi telegramApi)
        {
            ArgumentNullException.ThrowIfNull(telegramApi);
            return new(telegramApi, CacheApi.Instance);
        }
    }
}