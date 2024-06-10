using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class ProjectJson : IProjectJson, IProjectDataverseInputBuilder
{
    private const string EntityPluralName = "gg_projects";

    private const string FieldProjectName = "gg_name";

    public static DataverseEntityGetIn BuildDataverseEntityGetIn(Guid projectId)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(projectId),
            selectFields: [FieldProjectName]);

    public string? GetName()
        =>
        ProjectName;

    public string GetLookupValue()
        =>
        $"/{EntityPluralName}({Id:D})";

    [JsonPropertyName("gg_projectid")]
    public Guid Id { get; init; }

    [JsonPropertyName(FieldProjectName)]
    public string? ProjectName { get; init; }
}
