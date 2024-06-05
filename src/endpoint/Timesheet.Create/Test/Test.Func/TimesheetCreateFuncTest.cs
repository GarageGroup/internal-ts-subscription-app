using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Create.Test;

public static partial class TimesheetCreateFuncTest
{
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

    private static Mock<IDataverseEntityCreateSupplier> BuildMockDataverseCreateApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        _ = mock
            .Setup(static a => a.CreateEntityAsync(It.IsAny<DataverseEntityCreateIn<TimesheetJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }

    private static Mock<IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>> BuildMockDataverseApi(
        IDataverseEntityCreateSupplier dataverseSearchSupplier)
    {
        var mock = new Mock<IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>>();

        _ = mock.Setup(static a => a.Impersonate(It.IsAny<Guid>())).Returns(dataverseSearchSupplier);

        return mock;
    }
}