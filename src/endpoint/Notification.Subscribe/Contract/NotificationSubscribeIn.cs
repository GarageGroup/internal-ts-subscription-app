using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class NotificationSubscribeIn
{
    public NotificationSubscribeIn(
        [ClaimIn] long botId,
        [ClaimIn] long chatId, 
        [RootBodyIn] BaseSubscriptionData subscriptionData)
    {
        BotId = botId;
        ChatId = chatId;
        SubscriptionData = subscriptionData;
    }
    
    public long BotId { get; }

    public long ChatId { get; }

    public BaseSubscriptionData SubscriptionData { get; }
}