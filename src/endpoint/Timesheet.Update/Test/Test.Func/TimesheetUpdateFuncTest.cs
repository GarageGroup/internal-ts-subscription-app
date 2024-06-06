using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Update.Test;

public static partial class TimesheetUpdateFuncTest
{
    private static readonly TimesheetUpdateIn SomeInput
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

    private static Mock<IDataverseEntityUpdateSupplier> BuildMockDataverseUpdateApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityUpdateSupplier>();

        _ = mock
            .Setup(static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}