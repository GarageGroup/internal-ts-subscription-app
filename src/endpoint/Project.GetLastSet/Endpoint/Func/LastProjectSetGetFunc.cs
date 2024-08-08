using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class LastProjectSetGetFunc : ILastProjectSetGetFunc
{
    private static readonly DbParameterArrayFilter AllowedProjectTypeSetFilter;

    private static readonly DbRawFilter IncidentStateCodeFilter;

    private static readonly DbRawFilter LeadStateCodeFilter;

    private static readonly DbRawFilter OpportunityStateCodeFilter;

    private static readonly DbRawFilter ProjectStateCodeFilter;

    static LastProjectSetGetFunc()
    {
        AllowedProjectTypeSetFilter = DbLastProject.BuildAllowedProjectTypeSetFilter();
        IncidentStateCodeFilter = DbLastProject.BuildIncidentStateCodeFilter();
        LeadStateCodeFilter = DbLastProject.BuildLeadStateCodeFilter();
        OpportunityStateCodeFilter = DbLastProject.BuildOpportunityStateCodeFilter();
        ProjectStateCodeFilter = DbLastProject.BuildProjectStateCodeFilter();
    }

    private readonly ISqlQueryEntitySetSupplier sqlApi;

    private readonly ITodayProvider todayProvider;

    private readonly LastProjectSetGetOption option;

    internal LastProjectSetGetFunc(ISqlQueryEntitySetSupplier sqlApi, ITodayProvider todayProvider, LastProjectSetGetOption option)
    {
        this.sqlApi = sqlApi;
        this.todayProvider = todayProvider;
        this.option = option;
    }
}