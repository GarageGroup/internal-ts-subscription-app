using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static ProfileUpdateMetadata;

public sealed record class ProfileUpdateIn
{
    public ProfileUpdateIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn, SwaggerDescription(In.LanguageCodeDescription), StringExample(In.LanguageCodeExample)] ProfileLanguage? languageCode)
    {
        SystemUserId = systemUserId;
        LanguageCode = languageCode;
    }

    public Guid SystemUserId { get; }

    public ProfileLanguage? LanguageCode { get; }
}