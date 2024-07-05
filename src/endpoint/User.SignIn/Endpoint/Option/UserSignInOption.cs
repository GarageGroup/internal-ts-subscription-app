using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignInOption
{
    public UserSignInOption(long botId, [AllowNull] string botName, string botToken)
    {
        BotId = botId;
        BotName = botName.OrEmpty();
        BotToken = botToken.OrEmpty();
    }

    public long BotId { get; }

    public string BotName { get; }

    public string BotToken { get; }
}