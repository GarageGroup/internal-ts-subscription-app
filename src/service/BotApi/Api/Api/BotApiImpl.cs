using GarageGroup.Infra.Telegram.Bot;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class BotApiImpl(IBotUserApiSupplier telegramApi, ICacheApi cacheApi) : IBotApi
{
}