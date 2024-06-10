using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Tag.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TagSetGetFuncDependency
{
    public static Dependency<TagSetGetEndpoint> UseTagGetSetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TagSetGetEndpoint.Resolve);

        static TagSetGetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            return new(sqlApi);
        }
    }
}