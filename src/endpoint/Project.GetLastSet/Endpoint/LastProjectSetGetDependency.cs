using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Project.LastSetGet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class LastProjectSetGetDependency
{
    public static Dependency<LastProjectSetGetEndpoint> UseLastProjectSetGetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(LastProjectSetGetEndpoint.Resolve);

        static LastProjectSetGetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            return new(sqlApi);
        }
    }
}