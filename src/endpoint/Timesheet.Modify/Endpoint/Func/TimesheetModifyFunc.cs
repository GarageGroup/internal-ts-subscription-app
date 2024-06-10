using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class TimesheetModifyFunc(IDataverseApiClient dataverseApi) : ITimesheetCreateFunc, ITimesheetUpdateFunc
{
    private static Result<TimesheetJson, Failure<TFailureCode>> BindProjectOrFailure<TFailureCode>(
        TimesheetJson timesheet, TimesheetProject project)
        where TFailureCode : struct
    {
        if (project.Type is ProjectType.Project)
        {
            return timesheet with
            {
                ProjectLookupValue = TimesheetJson.BuildProjectLookupValue(project.Id)
            };
        }

        if (project.Type is ProjectType.Opportunity)
        {
            return timesheet with
            {
                OpportunityLookupValue = TimesheetJson.BuildOpportunityLookupValue(project.Id)
            };
        }

        if (project.Type is ProjectType.Lead)
        {
            return timesheet with
            {
                LeadLookupValue = TimesheetJson.BuildLeadLookupValue(project.Id)
            };
        }

        if (project.Type is ProjectType.Incident)
        {
            return timesheet with
            {
                IncidentLookupValue = TimesheetJson.BuildIncidentLookupValue(project.Id)
            };
        }

        return Failure.Create<TFailureCode>(default, $"An unexpected project type: {project.Type}");
    }
}