using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Timesheet.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TimesheetSetGetDependency
{
    public static Dependency<TimesheetSetGetEndpoint> UseTimesheetSetGetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetSetGetEndpoint.Resolve);

        static TimesheetSetGetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);

            return new(sqlApi);
        }
    }
}