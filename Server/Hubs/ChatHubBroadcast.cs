using Domain.DTO;
using Domain.Models;
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
        _notificationsServices.MemberCreated += OnMemberCreated;
        _notificationsServices.MemberRemoved += OnMemberRemoved;
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

            if (connectionSettings.UserName is null)
            {
                await SendChatsInfoToUnauthenticatedUser(connectionId, e.Chats, connectionSettings.NumberOfChats, connectionSettings.ChatSearch);
            }
            else
            {
                await SendChatsInfoToAuthenticatedUser(connectionId, connectionSettings.UserName, e.Chats, connectionSettings.NumberOfChats, connectionSettings.ChatSearch);
            }
        }
    }
    private async void OnMemberCreated(object? sender, MemberCreatedEventArgs e)
    {
        // TODO:
    }
    private async void OnMemberRemoved(object? sender, MemberRemovedEventArgs e)
    {
        // TODO:
    }

    private async Task SendChatsInfoToUnauthenticatedUser(string connectionId, IEnumerable<Chat> chats, int? numberOfChats, string? search)
    {
        IEnumerable<Chat> chatsInfo = new List<Chat>(chats);

        if (string.IsNullOrWhiteSpace(search) == false)
        {
            chatsInfo = chatsInfo.Where(chat => chat.Name.ToLower().Contains(search.ToLower().Trim()));
        }

        if (numberOfChats is not null)
        {
            chatsInfo = chatsInfo.Take((int)numberOfChats);
        }

        await SendChatsInfo(connectionId, chatsInfo.Select(chat => chat.ToDTO()));
    }
    private async Task SendChatsInfoToAuthenticatedUser(string connectionId, string userName, IEnumerable<Chat> chats, int? numberOfChats, string? search)
    {
        await SendOwnChatsInfo(connectionId, chats.Where(chat => chat.Author.Equals(userName)).Select(chat => chat.ToDTO()));

        IEnumerable<Chat> chatsInfo = new List<Chat>(chats.Where(chat => chat.Author.Equals(userName) == false));

        if (string.IsNullOrWhiteSpace(search) == false)
        {
            chatsInfo = chatsInfo.Where(chat => chat.Name.ToLower().Contains(search.ToLower().Trim()));
        }

        if (numberOfChats is not null)
        {
            chatsInfo = chatsInfo.Take((int)numberOfChats);
        }

        await SendOthersChatsInfo(connectionId, chatsInfo.Select(chat => chat.ToDTO()));
    }

    private async Task SendChatsInfo(string connectionId, IEnumerable<ChatDTO> chatsInfo)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync(ChatHub.ReceiveChatsInfoMethod, chatsInfo);
    }
    private async Task SendOwnChatsInfo(string connectionId, IEnumerable<ChatDTO> chatsInfo)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync(ChatHub.ReceiveOwnChatsInfoMethod, chatsInfo);
    }
    private async Task SendOthersChatsInfo(string connectionId, IEnumerable<ChatDTO> chatsInfo)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync(ChatHub.ReceiveOthersChatsInfoMethod, chatsInfo);
    }
}
