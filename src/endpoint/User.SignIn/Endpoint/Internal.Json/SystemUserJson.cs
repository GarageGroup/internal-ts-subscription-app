using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal readonly record struct SystemUserJson
{
    private const string EntityPluralName = "systemusers";

    private const string FullNameFieldName = "yomifullname";

    internal static DataverseEntityGetIn BuildDataverseInput(Guid systemUserId)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(systemUserId),
            selectFields: [FullNameFieldName]);

    [JsonPropertyName(FullNameFieldName)]
    public string? FullName { get; init; }
}