using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Modify.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TimesheetModifyDependency
{
    public static Dependency<TimesheetCreateEndpoint> UseTimesheetCreateEndpoint(
        this Dependency<IDataverseApiClient> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetCreateEndpoint.Resolve);
    }

    public static Dependency<TimesheetUpdateEndpoint> UseTimesheetUpdateEndpoint(
        this Dependency<IDataverseApiClient> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetUpdateEndpoint.Resolve);
    }

    private static TimesheetModifyFunc CreateFunc(IDataverseApiClient dataverseApi)
    {
        ArgumentNullException.ThrowIfNull(dataverseApi);
        return new(dataverseApi);
    }
}