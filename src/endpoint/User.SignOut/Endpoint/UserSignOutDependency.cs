using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.User.SignOut.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class UserSignOutDependency
{
    public static Dependency<UserSignOutEndpoint> UseUserSignOutEndpoint<TDataverseApi, TBotApi>(
        this Dependency<TDataverseApi, TBotApi> dependency)
        where TDataverseApi : IDataverseEntityUpdateSupplier
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(UserSignOutEndpoint.Resolve);

        static UserSignOutFunc CreateFunc(TDataverseApi dataverseApi, TBotApi botApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(botApi);

            return new(dataverseApi, botApi);
        }
    }
}