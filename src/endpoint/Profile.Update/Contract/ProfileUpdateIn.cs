using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProfileUpdateIn
{
    public ProfileUpdateIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] [StringExample("en")] string? languageCode)
    {
        SystemUserId = systemUserId;
        LanguageCode = languageCode;
    }

    public Guid SystemUserId { get; }

    public string? LanguageCode { get; }
}