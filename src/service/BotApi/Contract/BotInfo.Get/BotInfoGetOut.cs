using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class BotInfoGetOut
{
    public BotInfoGetOut(long id, [AllowNull] string username)
    {
        Id = id;
        Username = username.OrEmpty();
    }

    public long Id { get; }

    public string Username { get; }
}