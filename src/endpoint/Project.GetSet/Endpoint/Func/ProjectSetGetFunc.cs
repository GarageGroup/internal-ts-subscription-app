using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectSetGetFunc(ISqlQueryEntitySetSupplier sqlApi) : IProjectSetGetFunc
{
    private static readonly DbSelectQuery DbIncidentQueryAll
        =
        DbIncident.QueryAll with
        {
            Filter = DbIncident.IsActiveFilter
        };
}