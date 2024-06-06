using GarageGroup.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Tag.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class TagGetSetFuncDependency
{
    public static Dependency<TagGetSetEndpoint> UseTagGetSetEndpoint<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(TagGetSetEndpoint.Resolve);

        static TagGetSetFunc CreateFunc(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            return new(sqlApi);
        }
    }
}