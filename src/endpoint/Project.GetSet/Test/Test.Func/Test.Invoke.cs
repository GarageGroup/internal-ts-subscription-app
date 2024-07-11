using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.SetGet.Test;

partial class ProjectSetGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectMockSqlApiProjectCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        _ = await func.InvokeAsync(default, default);

        var expectedProjectQuery = new DbSelectQuery("gg_project", "p")
        {
            SelectedFields = new(
                "p.gg_projectid AS ProjectId",
                "p.gg_name AS ProjectName")
        };
        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbProject>(expectedProjectQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_ExpectMockSqlApiOpportunityCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        _ = await func.InvokeAsync(default, default);

        var expectedOpportunityQuery = new DbSelectQuery("opportunity", "o")
        {
            SelectedFields = new(
                "o.opportunityid AS ProjectId",
                "o.name AS ProjectName"),
        };
        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbOpportunity>(expectedOpportunityQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_ExpectMockSqlApiLeadCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        _ = await func.InvokeAsync(default, default);

        var expectedLeadQuery = new DbSelectQuery("lead", "l")
        {
            SelectedFields = new(
                "l.leadid AS ProjectId",
                "l.companyname AS CompanyName",
                "l.subject AS Subject"),
        };
        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbLead>(expectedLeadQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_ExpectMockSqlApiIncidentCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        _ = await func.InvokeAsync(default, default);

        var expectedIncidentQuery = new DbSelectQuery("incident", "i")
        {
            SelectedFields = new(
                "i.incidentid AS ProjectId",
                "i.title AS ProjectName"),
            Filter = new DbRawFilter("i.statecode = 0")
        };
        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbIncident>(expectedIncidentQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_DbProjectResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, dbFailure, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(default, default);
        var expected = Failure.Create(Unit.Value, "Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DbOpportunityResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, dbFailure, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(default, default);
        var expected = Failure.Create(Unit.Value, "Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DbLeadResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(SomeDbIncidentOutput, SomeDbProjectOutput, SomeDbOpportunityOutput, dbFailure);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(default, default);
        var expected = Failure.Create(Unit.Value, "Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DbIncidentResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(dbFailure, SomeDbProjectOutput, SomeDbOpportunityOutput, SomeDbLeadOutput);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(default, default);
        var expected = Failure.Create(Unit.Value, "Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProjectSetGetFuncSource.OutputGetTestData), MemberType = typeof(ProjectSetGetFuncSource))]
    internal static async Task InvokeAsync_DbResultIsSuccess_ExpectSuccess(
        FlatArray<DbIncident> dbIncidents,
        FlatArray<DbProject> dbProjects,
        FlatArray<DbOpportunity> dbOpportunities,
        FlatArray<DbLead> dbLeads,
        ProjectSetGetOut expected)
    {
        var mockSqlApi = BuildMockSqlApi(dbIncidents, dbProjects, dbOpportunities, dbLeads);
        var func = new ProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(default, default);
        Assert.StrictEqual(expected, actual);
    }
}
