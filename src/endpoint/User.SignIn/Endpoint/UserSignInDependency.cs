using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.User.SignIn.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class UserSignInDependency
{
    public static Dependency<UserSignInEndpoint> UseUserSignInEndpoint(
        this Dependency<IDataverseApiClient, UserSignInOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(UserSignInEndpoint.Resolve);

        static UserSignInFunc CreateFunc(IDataverseApiClient dataverseApi, UserSignInOption option)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(dataverseApi, option);
        }
    }
}