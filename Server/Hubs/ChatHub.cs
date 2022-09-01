using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;

namespace Server.Hubs;

public partial class ChatHub : Hub
{
    private readonly IStatisticsServices _statisticsServices;
    private readonly IIdentityServices _identityServices;
    private readonly IChatServices _chatServices;

    public ChatHub(IStatisticsServices statisticsServices, IIdentityServices identityServices, IChatServices chatServices)
    {
        _statisticsServices = statisticsServices;
        _identityServices = identityServices;
        _chatServices = chatServices;
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
