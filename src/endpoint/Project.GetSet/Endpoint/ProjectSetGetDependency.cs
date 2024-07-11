using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Project.SetGet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProjectSetGetDependency
{
    public static Dependency<ProjectSetGetEndpoint> UseProjectSetGetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(ProjectSetGetEndpoint.Resolve);

        static ProjectSetGetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);

            return new(sqlApi);
        }
    }
}