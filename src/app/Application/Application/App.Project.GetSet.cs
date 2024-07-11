using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<ProjectSetGetEndpoint> UseProjectSetGetEndpoint()
        =>
        UseSqlApi().UseProjectSetGetEndpoint();
}