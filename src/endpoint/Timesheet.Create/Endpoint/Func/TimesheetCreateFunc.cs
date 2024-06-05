using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class TimesheetCreateFunc(IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier> dataverseApi) : ITimesheetCreateFunc
{
    private static Result<TimesheetJson, Failure<TimesheetCreateFailureCode>> BindProjectOrFailure(
        TimesheetJson timesheet, TimesheetProject project)
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

        return Failure.Create(TimesheetCreateFailureCode.Unknown, $"An unexpected project type: {project.Type}");
    }
}