using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Timesheet.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class LastProjectGetSetDependency
{
    public static Dependency<TimesheetGetSetEndpoint> UseTimesheetGetSetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetGetSetEndpoint.Resolve);

        static TimesheetGetSetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);

            return new(sqlApi);
        }
    }
}