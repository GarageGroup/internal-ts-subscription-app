using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignInIn
{
    public UserSignInIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] string telegramData)
    {
        SystemUserId = systemUserId;
        TelegramData = telegramData;
    }

    public Guid SystemUserId { get; }

    public string TelegramData { get; }
}