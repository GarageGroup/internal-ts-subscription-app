using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProfile
{
    internal static DbCombinedFilter BuildDefaultFilter(Guid systemUserId, long botId)
        =>
        new(DbLogicalOperator.And)
        {
            Filters =
            [
                new DbParameterFilter($"{AliasName}.gg_systemuser_id", DbFilterOperator.Equal, systemUserId, "systemUserId"),
                new DbParameterFilter($"{AliasName}.gg_bot_id", DbFilterOperator.Equal, botId, "botId")
            ]
        };
}
