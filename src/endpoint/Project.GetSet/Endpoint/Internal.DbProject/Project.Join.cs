using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProject
{
    internal static FlatArray<DbAppliedTable> BuildTimesheetDateDbAppliedTables(Guid userId, DateOnly minDate)
        => 
        [
            new(
                type: DbApplyType.Outer,
                alias: UserLastTimesheetDateAliasName,
                selectQuery: new("gg_timesheetactivity", "t")
                {
                    SelectedFields = new("t.gg_date AS LastDay"),
                    Top = 1,
                    Filter = new DbCombinedFilter(DbLogicalOperator.And)
                    {
                        Filters =
                        [
                            new DbParameterFilter("t.ownerid", DbFilterOperator.Equal, userId, "userId"),
                            new DbParameterFilter("t.gg_date", DbFilterOperator.GreaterOrEqual, minDate.ToString("yyyy-MM-dd"), "minDate"),
                            new DbRawFilter("p.gg_projectid = t.regardingobjectid"),
                            new DbRawFilter($"t.regardingobjecttypecode = {ProjectType.Project:D}"),
                            new DbRawFilter("t.statecode = 0")
                        ]
                    },
                }),
            new(
                type: DbApplyType.Outer,
                alias: LastTimesheetDateAliasName,
                selectQuery: new("gg_timesheetactivity", "t1")
                {
                    SelectedFields = new("t1.gg_date AS LastDay"),
                    Top = 1,
                    Filter = new DbCombinedFilter(DbLogicalOperator.And)
                    {
                        Filters =
                        [
                            new DbParameterFilter("t1.ownerid", DbFilterOperator.Inequal, userId, "userId"),
                            new DbParameterFilter("t1.gg_date", DbFilterOperator.GreaterOrEqual, minDate.ToString("yyyy-MM-dd"), "minDate"),
                            new DbRawFilter("p.gg_projectid = t1.regardingobjectid"),
                            new DbRawFilter($"t1.regardingobjecttypecode = {ProjectType.Project:D}"),
                            new DbRawFilter("t1.statecode = 0")
                        ]
                    },
                })
        ];
}