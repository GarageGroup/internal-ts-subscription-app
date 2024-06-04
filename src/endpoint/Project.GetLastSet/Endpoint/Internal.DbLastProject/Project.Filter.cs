using GarageGroup.Infra;
using System;
using System.Linq;

namespace GarageGroup.Internal.Timesheet;

partial record class DbLastProject
{
    internal static DbRawFilter BuildIncidentStateCodeFilter()
        =>
        new($"({AliasName}.regardingobjecttypecode <> {IncidentEntityCode} " +
            $"OR EXISTS (SELECT TOP 1 1 FROM incident AS i WHERE {AliasName}.regardingobjectid = i.incidentid AND i.statecode = 0))");

    internal static DbParameterArrayFilter BuildAllowedProjectTypeSetFilter()
    {
        return new(
            fieldName: ProjectTypeCodeFieldName,
            @operator: DbArrayFilterOperator.In,
            fieldValues: Enum.GetValues<ProjectType>().Select(AsInt32).OrderBy(Pipeline.Pipe).Select(AsObject).ToFlatArray(),
            parameterPrefix: "projectTypeCode");

        static int AsInt32(ProjectType type)
            =>
            type switch
            {
                ProjectType.Opportunity => 3,
                ProjectType.Lead => 4,
                ProjectType.Incident => 112,
                _ => 10912
            };

        static object? AsObject(int type)
            =>
            type;
    }

    internal static DbParameterFilter BuildOwnerFilter(Guid ownerId)
        =>
        new($"{AliasName}.ownerid", DbFilterOperator.Equal, ownerId, "ownerId");

    internal static DbParameterFilter BuildMinDateFilter(DateOnly minDate)
    {
        
        return new($"{AliasName}.gg_date", DbFilterOperator.GreaterOrEqual, minDate.ToString("yyyy-MM-dd"), "minDate");
    }
}