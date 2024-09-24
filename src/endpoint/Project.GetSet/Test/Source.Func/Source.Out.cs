using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.SetGet.Test;

using DbIncidentArray = FlatArray<DbIncident>;
using DbProjectArray = FlatArray<DbProject>;
using DbOpportunityArray = FlatArray<DbOpportunity>;
using DbLeadArray = FlatArray<DbLead>;

partial class ProjectSetGetFuncSource
{
    public static TheoryData<DbIncidentArray, DbProjectArray, DbOpportunityArray, DbLeadArray, ProjectSetGetOut> OutputGetTestData
        =>
        new()
        {
            {
                default,
                default,
                default,
                default,
                default
            },
            {
                [
                    new()
                    {
                        ProjectId = new("1689833a-1a3c-48ce-8f8b-d743cca273d9"),
                        ProjectName = "First incident name"
                    },
                    new()
                    {
                        ProjectId = new("41bdefb5-52f6-4954-922e-dbc233b9e26d"),
                        ProjectName = "Second incident name"
                    }
                ],
                [
                    new()
                    {
                        ProjectId = new("cf5ffdf3-a045-4eb5-a0e6-67593af34711"),
                        ProjectName = "First project name",
                        ProjectComment = null,
                    },
                    new()
                    {
                        ProjectId = new("3411f4c8-3de5-4cac-9b69-0b7f39214bbe"),
                        ProjectName = "Second project name",
                        ProjectComment = "\n\r"
                    },
                    new()
                    {
                        ProjectId = new("75468ddc-961f-45d5-8c98-7e0d60a9e404"),
                        ProjectName = "Third project",
                        ProjectComment = "Some project comment"
                    }
                ],
                [
                    new()
                    {
                        ProjectId = new("7ac32f69-35c3-46f6-a0bf-dd1085f9323a"),
                        ProjectName = "First opportunity name"
                    },
                    new()
                    {
                        ProjectId = new("1bf452eb-8246-45aa-959b-19eba14f4f83"),
                        ProjectName = "Second opportunity name"
                    }
                ],
                [
                    new()
                    {
                        ProjectId = new("61313de5-91ca-4216-9077-09701382cacc"),
                        CompanyName = "Company name",
                        Subject = "Subject"
                    },
                    new()
                    {
                        ProjectId = new("00ad0b9e-a708-40da-80d9-9a6e3f5b0c63"),
                        CompanyName = null,
                        Subject = "Subject"
                    },
                    new()
                    {
                        ProjectId = new("ff891399-0c63-4ff7-aefc-2ce738cf49ed"),
                        CompanyName = "",
                        Subject = "Subject"
                    }
                ],
                new()
                {
                    Projects =
                    [
                        new(
                            id: new("1689833a-1a3c-48ce-8f8b-d743cca273d9"),
                            name: "First incident name",
                            type: ProjectType.Incident),
                        new(
                            id: new("7ac32f69-35c3-46f6-a0bf-dd1085f9323a"),
                            name: "First opportunity name",
                            type: ProjectType.Opportunity),
                        new(
                            id: new("cf5ffdf3-a045-4eb5-a0e6-67593af34711"),
                            name: "First project name",
                            type: ProjectType.Project),
                        new(
                            id: new("41bdefb5-52f6-4954-922e-dbc233b9e26d"),
                            name: "Second incident name",
                            type: ProjectType.Incident),
                        new(
                            id: new("1bf452eb-8246-45aa-959b-19eba14f4f83"),
                            name: "Second opportunity name",
                            type: ProjectType.Opportunity),
                        new(
                            id: new("3411f4c8-3de5-4cac-9b69-0b7f39214bbe"),
                            name: "Second project name",
                            type: ProjectType.Project),
                        new(
                            id: new("00ad0b9e-a708-40da-80d9-9a6e3f5b0c63"),
                            name: "Subject",
                            type: ProjectType.Lead),
                        new(
                            id: new("ff891399-0c63-4ff7-aefc-2ce738cf49ed"),
                            name: "Subject",
                            type: ProjectType.Lead),
                        new(
                            id: new("61313de5-91ca-4216-9077-09701382cacc"),
                            name: "Subject (Company name)",
                            type: ProjectType.Lead),
                        new(
                            id: new("75468ddc-961f-45d5-8c98-7e0d60a9e404"),
                            name: "Third project",
                            type: ProjectType.Project)
                        {
                            Comment = "Some project comment"
                        }
                    ]
                }
            }
        };
}
