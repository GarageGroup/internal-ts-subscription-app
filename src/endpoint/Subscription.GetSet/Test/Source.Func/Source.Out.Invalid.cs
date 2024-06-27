using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

partial class SubscriptionSetGetFuncSource
{
    public static TheoryData<FlatArray<SubscriptionJson>, string> OutputInvalidTestData
        =>
        new()
        {
            {
                [
                    new()
                    {
                        NotificationType = null
                    }
                ],
                "An unknown notification type: ''"
            },
            {
                [
                    new()
                    {
                        NotificationType = new()
                        {
                            Key = null
                        }
                    }
                ],
                "An unknown notification type: ''"
            },
            {
                [
                    new()
                    {
                        NotificationType = new()
                        {
                            Key = "SomeType"
                        }
                    }
                ],
                "An unknown notification type: 'SomeType'"
            }
        };
}