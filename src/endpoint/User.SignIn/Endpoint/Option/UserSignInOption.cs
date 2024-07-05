namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignInOption
{
    public required long BotId { get; init; }

    public required string BotName { get; init; }

    public required string BotToken { get; init; }
}