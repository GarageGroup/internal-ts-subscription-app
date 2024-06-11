using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Profile.Get.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProfileGetDependency
{
    public static Dependency<ProfileGetEndpoint> UseProfileGetEndpoint<TSqlApi>(
        this Dependency<TSqlApi, ProfileGetOption> dependency)
        where TSqlApi : ISqlQueryEntitySupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(ProfileGetEndpoint.Resolve);

        static ProfileGetFunc CreateFunc(TSqlApi dataverseApi, ProfileGetOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(dataverseApi, option);
        }
    }
}