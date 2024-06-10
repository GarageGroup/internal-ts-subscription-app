using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<TagSetGetEndpoint> UseTagSetGetEndpoint()
        =>
        UseSqlApi().UseTagGetSetEndpoint();
}