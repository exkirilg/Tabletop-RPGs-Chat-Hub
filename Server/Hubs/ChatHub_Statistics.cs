using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public partial class ChatHub
{
    public const string ReceiveStatisticsMethod = "ReceiveStatistics";

    public async Task StatisticsRequest()
    {
        await SendStatisticsToCurrentConnection();
    }

    private async Task SendStatisticsToAll()
    {
        await Clients.All.SendAsync(ReceiveStatisticsMethod, await _statisticsServices.GetStatistics());
    }
    private async Task SendStatisticsToCurrentConnection()
    {
        await Clients.Client(Context.ConnectionId).SendAsync(ReceiveStatisticsMethod, await _statisticsServices.GetStatistics());
    }
}
