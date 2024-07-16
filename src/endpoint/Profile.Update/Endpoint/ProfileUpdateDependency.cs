using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProfileUpdateDependency
{
    public static Dependency<ProfileUpdateEndpoint> UseProfileUpdateEndpoint<TDataverseApi, TBotApi>(
        this Dependency<TDataverseApi, TBotApi> dependency)
        where TDataverseApi : IDataverseEntityUpdateSupplier
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(ProfileUpdateEndpoint.Resolve);

        static ProfileUpdateFunc CreateFunc(TDataverseApi dataverseApi, TBotApi botApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(botApi);

            return new(dataverseApi, botApi);
        }
    }
}