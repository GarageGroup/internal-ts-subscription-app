using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class LastProjectSetGetFunc : ILastProjectSetGetFunc
{
    private static readonly DbParameterArrayFilter AllowedProjectTypeSetFilter;

    private static readonly DbRawFilter IncidentStateCodeFilter;

    static LastProjectSetGetFunc()
    {
        AllowedProjectTypeSetFilter = DbLastProject.BuildAllowedProjectTypeSetFilter();
        IncidentStateCodeFilter = DbLastProject.BuildIncidentStateCodeFilter();
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