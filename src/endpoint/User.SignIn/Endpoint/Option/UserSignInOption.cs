using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignInOption
{
    public UserSignInOption(string botToken)
        =>
        BotToken = botToken.OrEmpty();

    public string BotToken { get; }
}