using GarageGroup.Infra;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

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
    public string UserName { get; }

    [JsonBodyOut]
    public string LanguageCode { get; }
}