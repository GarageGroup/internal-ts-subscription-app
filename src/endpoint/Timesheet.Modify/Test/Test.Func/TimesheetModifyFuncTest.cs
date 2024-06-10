using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Modify.Test;

public static partial class TimesheetModifyFuncTest
{
    private static readonly DataverseEntityGetOut<ProjectJson> SomeProjectJsonOut
        =
        new(
            new ProjectJson()
            {
                Id = new("7babe661-cc69-474a-95a5-82ce45204ff8"),
                ProjectName = "Some project name"
            });

    private static readonly DataverseEntityGetOut<IncidentJson> SomeIncidentJsonOut
        =
        new(
            new IncidentJson()
            {
                Id = new("7babe661-cc69-474a-95a5-82ce45204ff8"),
                ProjectName = "Some project name"
            });

    private static readonly DataverseEntityGetOut<OpportunityJson> SomeOpportunityJsonOut
        =
        new(
            new OpportunityJson()
            {
                Id = new("7babe661-cc69-474a-95a5-82ce45204ff8"),
                ProjectName = "Some project name"
            });

    private static readonly DataverseEntityGetOut<LeadJson> SomeLeadJsonOut
        =
        new(
            new LeadJson()
            {
                Id = new("7babe661-cc69-474a-95a5-82ce45204ff8"),
                CompanyName = "Some company",
                Subject = "Some subject"
            });

    private static Mock<IDataverseApiClient> BuildMockDataverseApi<TOut>(
        in Result<Unit, Failure<DataverseFailureCode>> result,
        in Result<DataverseEntityGetOut<TOut>, Failure<DataverseFailureCode>> getProjectNameResult)
        where TOut : IProjectJson, IProjectDataverseInputBuilder
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock.Setup(static a => a.Impersonate(It.IsAny<Guid>())).Returns(mock.Object);

        _ = mock
            .Setup(static a => a.CreateEntityAsync(It.IsAny<DataverseEntityCreateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ = mock
            .Setup(static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ = mock
            .Setup(static a => a.GetEntityAsync<TOut>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getProjectNameResult);

        return mock;
    }
}