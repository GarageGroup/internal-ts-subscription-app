using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc : INotificationSubscribeFunc
{
    private readonly IDataverseApiClient dataverseApi;
    
    internal NotificationSubscribeFunc(IDataverseApiClient dataverseApi) 
        => this.dataverseApi = dataverseApi;
}