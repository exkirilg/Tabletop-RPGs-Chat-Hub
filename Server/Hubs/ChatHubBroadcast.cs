using Microsoft.AspNetCore.SignalR;
using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Server.Hubs;

public class ChatHubBroadcast
{
	private readonly IHubContext<ChatHub> _hubContext;
    private readonly INotificationsServices _notificationsServices;

    public ChatHubBroadcast(IHubContext<ChatHub> hubContext, INotificationsServices notificationsServices)
	{
        _hubContext = hubContext;
        _notificationsServices = notificationsServices;

        _notificationsServices.StatisticsChanged += OnStatisticsChanged;
    }

    private async void OnStatisticsChanged(object? sender, StatisticsChangedEventArgs e)
    {
       await _hubContext.Clients.All.SendAsync(ChatHub.ReceiveStatisticsMethod, e.StatisticsDTO);
    }
}
