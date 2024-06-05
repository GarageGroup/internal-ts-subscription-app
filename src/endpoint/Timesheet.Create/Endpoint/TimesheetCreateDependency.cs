using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Create.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TimesheetCreateDependency
{
    public static Dependency<TimesheetCreateEndpoint> UseTimesheetCreateEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi> dependency)
        where TDataverseApi : IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TimesheetCreateEndpoint.Resolve);

        static TimesheetCreateFunc CreateFunc(TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}