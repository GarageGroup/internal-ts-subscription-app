using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Delete.Test;

internal static partial class TimesheetDeleteFuncSource
{
    public static TheoryData<TimesheetDeleteIn, DataverseEntityDeleteIn> InputTestData
        =>
        new()
        {
            {
                new(
                    timesheetId: new("17bdba90-1161-4715-b4bf-b416200acc79")),
                new(
                    entityPluralName: "gg_timesheetactivities",
                    entityKey: new DataversePrimaryKey(new("17bdba90-1161-4715-b4bf-b416200acc79")))
            },
            {
                new(
                    timesheetId: new("4835096d-03ef-4e30-abc1-77bcfe3a5d5f")),
                new(
                    entityPluralName: "gg_timesheetactivities",
                    entityKey: new DataversePrimaryKey(new("4835096d-03ef-4e30-abc1-77bcfe3a5d5f")))
            },
            {
                default,
                new(
                    entityPluralName: "gg_timesheetactivities",
                    entityKey: new DataversePrimaryKey(default))
            }
        };
}