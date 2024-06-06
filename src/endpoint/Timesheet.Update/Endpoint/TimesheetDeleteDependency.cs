using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Update.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TimesheetDeleteDependency
{
    public static Dependency<TimesheetUpdateEndpoint> UseTimesheetUpdateEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi> dependency)
        where TDataverseApi : IDataverseEntityUpdateSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetUpdateEndpoint.Resolve);

        static TimesheetUpdateFunc CreateFunc(TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}