using System.Text.Json;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.SearchSet.Test;

internal static partial class ProjectSetSearchFuncSource
{
    public static TheoryData<DataverseSearchOut, ProjectSetSearchOut> OutputTestData
        =>
        new()
        {
            {
                new(
                    totalRecordCount: 1,
                    value: default),
                default
            },
            {
                new(
                    totalRecordCount: 3,
                    value:
                    [
                        new(
                            searchScore: 18.698789596557617,
                            objectId: new("cc1efd36-ceca-eb11-bacc-000d3a47050c"),
                            entityName: "opportunity",
                            extensionData: default),
                        new(
                            searchScore: 16.482242584228516,
                            objectId: new("93877469-68ca-eb11-bacc-000d3a47050c"),
                            entityName: "gg_project",
                            extensionData:
                            [
                                new("gg_name", new(JsonSerializer.SerializeToElement("some second project name")))
                            ]),
                        new(
                            searchScore: 1000,
                            objectId: new("5660cb5b-e3de-465a-9c2a-5a445c1faa1a"),
                            entityName: "incident",
                            extensionData:
                            [
                                new("title", new(JsonSerializer.SerializeToElement("Third Project")))
                            ]),
                        new(
                            searchScore: 1001,
                            objectId: new("308891d9-cdca-eb11-bacc-000d3a47050c"),
                            entityName: "Some",
                            extensionData: default),
                        new(
                            searchScore: 2000,
                            objectId: new("07dedef2-951c-4405-8e17-4338e7408690"),
                            entityName: "lead",
                            extensionData:
                            [
                                new("subject", new(JsonSerializer.SerializeToElement("Some test"))),
                                new("companyname", new(JsonSerializer.SerializeToElement("Some company name")))
                            ]),
                        new(
                            searchScore: 238,
                            objectId: new("07dedef2-951c-4405-8e17-4338e7408287"),
                            entityName: "lead",
                            extensionData:
                            [
                                new("subject", new(JsonSerializer.SerializeToElement("Some test with empty companyname")))
                            ]),
                        new(
                            searchScore: 238,
                            objectId: new("07dedef2-951c-4405-8e17-4338e7408237"),
                            entityName: "lead",
                            extensionData:
                            [
                                new("companyname", new(JsonSerializer.SerializeToElement("Some test with empty subject")))
                            ]),
                        new(
                            searchScore: 238,
                            objectId: new("07dedef2-951c-4405-8e17-4338e7408238"),
                            entityName: "lead",
                            extensionData: default)
                    ]),
                new()
                {
                    Projects =
                    [
                        new(
                            id: new("cc1efd36-ceca-eb11-bacc-000d3a47050c"),
                            name: string.Empty,
                            type: ProjectType.Opportunity),
                        new(
                            id: new("93877469-68ca-eb11-bacc-000d3a47050c"),
                            name: "some second project name",
                            type: ProjectType.Project),
                        new(
                            id: new("5660cb5b-e3de-465a-9c2a-5a445c1faa1a"),
                            name: "Third Project",
                            type: ProjectType.Incident),
                        new(
                            id: new("07dedef2-951c-4405-8e17-4338e7408690"),
                            name: "Some test (Some company name)",
                            type: ProjectType.Lead),
                        new(
                            id: new("07dedef2-951c-4405-8e17-4338e7408287"),
                            name: "Some test with empty companyname",
                            type: ProjectType.Lead),
                        new(
                            id: new("07dedef2-951c-4405-8e17-4338e7408237"),
                            name: "(Some test with empty subject)",
                            type: ProjectType.Lead),
                        new(
                            id: new("07dedef2-951c-4405-8e17-4338e7408238"),
                            name: string.Empty,
                            type: ProjectType.Lead)
                    ]
                }
            }
        };
}