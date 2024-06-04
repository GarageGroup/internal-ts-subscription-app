using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.LastSetGet.Test;

public static partial class LastProjectSetGetFuncTest
{
    private static readonly LastProjectSetGetIn SomeGetLastInput
        =
        new(
            systemUserId: new("45b6e085-4d6e-4b2d-a26c-eb8c1c5a2e5e"),
            top: 10,
            minDate: new(2023, 07, 25));

    private static readonly FlatArray<DbLastProject> SomeDbLastProjectOutput
        =
        [
            new()
            {
                ProjectId = new("63d9e1b7-706b-ea11-a813-000d3a44ad35"),
                ProjectName = "Some project name",
                ProjectTypeCode = 10912,
                Subject = null
            },
            new()
            {
                ProjectId = new("20f2d7f3-c73d-4895-aa09-c8cdb3cd0acd"),
                ProjectName = "Some lead name",
                ProjectTypeCode = 4,
                Subject = "Some lead subject"
            }
        ];

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbLastProject>, Failure<Unit>> result)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(static a => a.QueryEntitySetOrFailureAsync<DbLastProject>(It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}