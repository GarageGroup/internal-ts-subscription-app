using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.User.SignOut.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class UserSignOutDependency
{
    public static Dependency<UserSignOutEndpoint> UseUserSignOutEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi, UserSignOutOption> dependency)
        where TDataverseApi : IDataverseEntityUpdateSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(UserSignOutEndpoint.Resolve);

        static UserSignOutFunc CreateFunc(TDataverseApi dataverseApi, UserSignOutOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(dataverseApi, option);
        }
    }
}