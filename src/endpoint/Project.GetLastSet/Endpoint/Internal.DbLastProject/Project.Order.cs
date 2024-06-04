using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbLastProject
{
    internal static readonly FlatArray<DbOrder> DefaultOrders
        =
        [
            new(nameof(MaxDate), DbOrderType.Descending),
            new(nameof(MaxCreatedOn), DbOrderType.Descending)
        ];
}