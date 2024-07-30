using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.LastSetGet.Test;

partial class LastProjectSetGetFuncSource
{
    public static TheoryData<LastProjectSetGetIn, LastProjectSetGetOption, DateOnly, DbSelectQuery> InputGetLastTestData
        =>
        new()
        {
            {
                new(
                    systemUserId: new("bef33be0-99f5-4018-ba80-3366ec9ec1fd"),
                    top: 7),
                new(lastDaysPeriod: 30),
                new(2024, 06, 04),
                new("gg_timesheetactivity", "t")
                {
                    Top = 7,
                    SelectedFields =
                    [
                        "t.regardingobjectid AS ProjectId",
                        "t.regardingobjecttypecode AS ProjectTypeCode",
                        "MAX(t.regardingobjectidname) AS ProjectName",
                        "(SELECT TOP 1 sub.subject " +
                        "FROM gg_timesheetactivity sub " +
                        "WHERE sub.regardingobjectid = t.regardingobjectid " +
                        "ORDER BY sub.createdon DESC) AS Subject",
                        "MAX(t.gg_date) AS MaxDate",
                        "MAX(t.createdon) AS MaxCreatedOn"
                    ],
                    Filter = new DbCombinedFilter(DbLogicalOperator.And)
                    {
                        Filters =
                        [
                            new DbParameterFilter(
                                fieldName: "t.ownerid",
                                @operator: DbFilterOperator.Equal,
                                fieldValue: Guid.Parse("bef33be0-99f5-4018-ba80-3366ec9ec1fd"),
                                parameterName: "ownerId"),
                            new DbParameterFilter(
                                fieldName: "t.gg_date",
                                @operator: DbFilterOperator.GreaterOrEqual,
                                fieldValue: "2024-05-05",
                                parameterName: "minDate"),
                            new DbParameterArrayFilter(
                                fieldName: "t.regardingobjecttypecode",
                                @operator: DbArrayFilterOperator.In,
                                fieldValues: new(3, 4, 112, 10912),
                                parameterPrefix: "projectTypeCode"),
                            new DbRawFilter("(t.regardingobjecttypecode <> 112 " +
                                "OR EXISTS (SELECT TOP 1 1 FROM incident AS i WHERE t.regardingobjectid = i.incidentid AND i.statecode = 0))")
                        ]
                    },
                    GroupByFields =
                    [
                        "t.regardingobjectid",
                        "t.regardingobjecttypecode"
                    ],
                    Orders =
                    [
                        new("MaxDate", DbOrderType.Descending),
                        new("MaxCreatedOn", DbOrderType.Descending)
                    ]
                }
            }
        };
}