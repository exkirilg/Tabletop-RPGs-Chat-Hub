using Microsoft.AspNetCore.SignalR;
using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Server.Hubs;

public class ChatHubBroadcast
{
	private readonly IHubContext<ChatHub> _hubContext;
    private readonly INotificationsServices _notificationsServices;
    private readonly State _state;

    public ChatHubBroadcast(IHubContext<ChatHub> hubContext, INotificationsServices notificationsServices, State state)
	{
        _hubContext = hubContext;
        _notificationsServices = notificationsServices;
        _state = state;

        _notificationsServices.StatisticsChanged += OnStatisticsChanged;
        _notificationsServices.ChatsChanged += OnChatsChanged;
    }

    private async void OnStatisticsChanged(object? sender, StatisticsChangedEventArgs e)
    {
       await _hubContext.Clients.All.SendAsync(ChatHub.ReceiveStatisticsMethod, e.StatisticsDTO);
    }
    private async void OnChatsChanged(object? sender, ChatsChangedEventArgs e)
    {
        foreach (var connectionId in _state.GetConnections())
        {
            var connectionSettings = _state.GetConnectionSettings(connectionId);

            var chats = e.Chats;
            if (string.IsNullOrWhiteSpace(connectionSettings.ChatsSearch) == false)
            {
                chats = chats.Where(chat => chat.Name.ToLower().Contains(connectionSettings.ChatsSearch.ToLower().Trim()));
            }
            if (connectionSettings.NumberOfChats is not null)
            {
                chats = chats.Take((int)connectionSettings.NumberOfChats);
            }

            await _hubContext.Clients.Client(connectionId).SendAsync(
                ChatHub.ReceiveChatsInfoMethod,
                chats.Select(chat => chat.ToDTO()));
        }
    }
}
