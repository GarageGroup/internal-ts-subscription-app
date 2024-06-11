using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProfile
{
    [DbSelect(All, AliasName, $"{AliasName}.gg_systemuser_idname")]
    public string? UserName { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.gg_language_code")]
    public string? LanguageCode { get; init; }
}