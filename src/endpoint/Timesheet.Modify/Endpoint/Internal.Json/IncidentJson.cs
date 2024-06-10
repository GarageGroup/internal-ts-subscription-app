using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class IncidentJson : IProjectJson, IProjectDataverseInputBuilder
{
    private const string EntityPluralName = "incidents";

    private const string FieldProjectName = "title";

    public static DataverseEntityGetIn BuildDataverseEntityGetIn(Guid incidentId)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(incidentId),
            selectFields: [FieldProjectName]);

    public string? GetName()
        =>
        ProjectName;

    public string GetLookupValue()
        =>
        $"/{EntityPluralName}({Id:D})";

    [JsonPropertyName("incidentid")]
    public Guid Id { get; init; }

    [JsonPropertyName(FieldProjectName)]
    public string? ProjectName { get; init; }
}
