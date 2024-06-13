using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProfileUpdateDependency
{
    public static Dependency<ProfileUpdateEndpoint> UseProfileUpdateEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi, ProfileUpdateOption> dependency)
        where TDataverseApi : IDataverseEntityUpdateSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(ProfileUpdateEndpoint.Resolve);

        static ProfileUpdateFunc CreateFunc(TDataverseApi dataverseApi, ProfileUpdateOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(dataverseApi, option);
        }
    }
}