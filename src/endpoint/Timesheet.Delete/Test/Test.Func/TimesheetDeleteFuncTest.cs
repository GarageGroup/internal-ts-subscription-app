using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Delete.Test;

public static partial class TimesheetDeleteFuncTest
{
    private static readonly TimesheetDeleteIn SomeInput
        =
        new(
            timesheetId: Guid.Parse("17bdba90-1161-4715-b4bf-b416200acc79"));

    private static Mock<IDataverseEntityDeleteSupplier> BuildMockDataverseDeleteApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityDeleteSupplier>();

        _ = mock
            .Setup(static a => a.DeleteEntityAsync(It.IsAny<DataverseEntityDeleteIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}