using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Endpoint.Profile.Get.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProfileGetDependency
{
    public static Dependency<ProfileGetEndpoint> UseProfileGetEndpoint<TSqlApi, TBotApi>(
        this Dependency<TSqlApi, TBotApi> dependency)
        where TSqlApi : ISqlQueryEntitySupplier
        where TBotApi : IBotInfoGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(ProfileGetEndpoint.Resolve);

        static ProfileGetFunc CreateFunc(TSqlApi dataverseApi, TBotApi botApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            ArgumentNullException.ThrowIfNull(botApi);

            return new(dataverseApi, botApi);
        }
    }
}