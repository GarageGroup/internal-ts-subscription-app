using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Modify.Test;

public static partial class TimesheetModifyFuncTest
{
    private static readonly TimesheetUpdateIn SomeTimesheetUpdateInput
        =
        new(
            timesheetId: Guid.Parse("80108b86-61ae-47ea-bd61-6d0c126a42b4"),
            date: new(2024, 03, 17),
            project: new(
                id: new("3dd8b1e0-3281-49f5-842b-cd1556113823"),
                type: ProjectType.Project,
                displayName: "Some project name"),
            duration: 8,
            description: "Some description");

    private static readonly TimesheetCreateIn SomeTimesheetCreateInput
        =
        new(
            systemUserId: new("56276a44-1444-4f67-bdb7-774b3f25932a"),
            date: new(2021, 10, 07),
            project: new(
                id: new("7583b4e6-23f5-eb11-94ef-00224884a588"),
                type: ProjectType.Project,
                displayName: "Some project name"),
            duration: 9,
            description: "Some description");

    private static Mock<IDataverseApiClient> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock.Setup(static a => a.Impersonate(It.IsAny<Guid>())).Returns(mock.Object);

        _ = mock
            .Setup(static a => a.CreateEntityAsync(It.IsAny<DataverseEntityCreateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ = mock
            .Setup(static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}