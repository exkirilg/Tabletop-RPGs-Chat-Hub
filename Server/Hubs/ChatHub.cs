using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;

namespace Server.Hubs;

public partial class ChatHub : Hub
{
    private readonly IStatisticsServices _statisticsServices;
    private readonly IChatServices _chatServices;

    private readonly ChatHubBroadcast _broadcast;

    public ChatHub(IStatisticsServices statisticsServices, IChatServices chatServices, ChatHubBroadcast broadcast)
    {
        _statisticsServices = statisticsServices;
        _chatServices = chatServices;
        _broadcast = broadcast;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
