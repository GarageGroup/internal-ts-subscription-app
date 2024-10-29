using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectSetGetFunc(ISqlQueryEntitySetSupplier sqlApi, ITodayProvider todayProvider) : IProjectSetGetFunc
{
    private const int LastDaysPeriod = 30;

    private static readonly DbSelectQuery DbIncidentQueryAll
        =
        DbIncident.QueryAll with
        {
            Filter = DbIncident.IsActiveFilter
        };

    private static readonly DbSelectQuery DbLeadQueryAll
        =
        DbLead.QueryAll with
        {
            Filter = DbLead.IsActiveFilter
        };

    private static readonly DbSelectQuery DbOpportunityQueryAll
        =
        DbOpportunity.QueryAll with
        {
            Filter = DbOpportunity.IsActiveFilter
        };
}