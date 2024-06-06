using GarageGroup.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class TagGetSetFunc
{
    public ValueTask<Result<TagGetSetOut, Failure<Unit>>> InvokeAsync(
        TagGetSetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbTag.QueryAll with
            {
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbTag.BuildOwnerFilter(@in.SystemUserId),
                        DbTag.BuildProjectFilter(@in.ProjectId),
                        DescriptionTagFilter,
                        DbTag.BuildMinDateFilter(@in.MinDate),
                        DbTag.BuildMaxDateFilter(@in.MaxDate)
                    ]
                },
                Orders = DbTag.DefaultOrders
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTag>)
        .MapSuccess(
            static success => new TagGetSetOut
            {
                Tags = success.AsEnumerable().SelectMany(GetHashTags).Distinct().ToFlatArray()
            });

    private static IEnumerable<string> GetHashTags(DbTag timesheet)
    {
        if (string.IsNullOrWhiteSpace(timesheet.Description))
        {
            yield break;
        }

        foreach (var tagMatch in TagRegex.Matches(timesheet.Description).Cast<Match>())
        {
            yield return tagMatch.Value;
        }
    }
}