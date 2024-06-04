using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Project.LastSetGet.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Internal.Timesheet;

public static class LastProjectSetGetDependency
{
    public static Dependency<LastProjectSetGetEndpoint> UseLastProjectSetGetEndpoint<TSqlApi>(
        this Dependency<TSqlApi, LastProjectSetGetOption> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(LastProjectSetGetEndpoint.Resolve);

        static LastProjectSetGetFunc CreateFunc(TSqlApi sqlApi, LastProjectSetGetOption option)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(sqlApi, TodayProvider.Instance, option);
        }
    }
}