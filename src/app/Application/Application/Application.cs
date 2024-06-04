using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;
using System;

namespace GarageGroup.Internal.Timesheet;

internal static partial class Application
{
    private static Dependency<IDataverseApiClient> UseDataverseApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("DataverseApi")
        .UseTokenCredentialStandard()
        .UsePollyStandard()
        .UseDataverseApiClient("Dataverse");

    private static Dependency<ISqlApi> UseSqlApi()
        =>
        DataverseDbProvider.Configure("Dataverse")
        .UseSqlApi();

    private static TLastProjectSetGetOption ResolveLastProjectSetGetOptionOrThrow<TLastProjectSetGetOption>(IServiceProvider serviceProvider)
        where TLastProjectSetGetOption : class
    {
        return serviceProvider.GetConfiguration().GetRequiredSection("Project").Get<TLastProjectSetGetOption>() ?? throw CreateException();

        static InvalidOperationException CreateException()
            =>
            new("Project option must be specified");
    }

    private static IConfiguration GetConfiguration(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<IConfiguration>();
}