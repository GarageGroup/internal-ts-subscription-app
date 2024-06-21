using GarageGroup.Infra;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

using static ProfileGetMetadata;

public sealed record class ProfileGetOut
{
    public ProfileGetOut(
        [AllowNull] string userName,
        [AllowNull] string languageCode)
    {
        UserName = userName.OrEmpty();
        LanguageCode = languageCode.OrEmpty();
    }

    [JsonBodyOut]
    [SwaggerDescription(Out.UserNameDescription)]
    [StringExample(Out.UserNameExample)]
    public string UserName { get; }

    [JsonBodyOut]
    [SwaggerDescription(Out.LanguageCodeDescription)]
    [StringExample(Out.LanguageCodeExample)]
    public string LanguageCode { get; }
}