using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.GetSet.Test;

public static partial class TimesheetSetGetFuncTest
{
    private static readonly TimesheetGetSetIn SomeTimesheetSetGetInput
        =
        new(
            systemUserId: new("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            date: new(2022, 02, 07));

    private static readonly FlatArray<DbTimesheet> SomeDbTimesheetSet
        =
        [
            new()
            {
                Duration = 8,
                ProjectName = "Some Opportunity",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit."
            },
            new()
            {
                Duration = 3,
                ProjectName = null,
                Subject = "Some prject name",
                Description = "Some text"
            }
        ];

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbTimesheet>, Failure<Unit>> result)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(static a => a.QueryEntitySetOrFailureAsync<DbTimesheet>(It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}