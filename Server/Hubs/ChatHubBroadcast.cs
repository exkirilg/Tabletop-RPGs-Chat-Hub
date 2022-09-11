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
        _notificationsServices.ChatRemoved += OnChatRemoved;
        _notificationsServices.MemberUpdated += OnMemberUpdated;
        _notificationsServices.NewMessage += OnNewMessage;
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
                await SendChatsInfoToUnauthenticatedUser(connectionId, e.Chats, connectionSettings.ChatSearch);
            }
            else
            {
                await SendChatsInfoToAuthenticatedUser(connectionId, connectionSettings.UserName, e.Chats, connectionSettings.ChatSearch);
            }
        }
    }
    private async void OnChatRemoved(object? sender, ChatRemovedEventArgs e)
    {
        _state.RemoveChat(e.Chat.ChatId);
        await _hubContext.Clients.Group(e.Chat.Name).SendAsync(ChatHub.ReceiveChatHasBeenRemovedMethod, e.Chat.ToDTO());
    }
    private void OnMemberUpdated(object? sender, MemberChangedEventArgs e)
    {
        _state.RemoveMember(e.Member.MemberId);
    }
    private async void OnNewMessage(object? sender, NewMessageEventArgs e)
    {
        if (e.Message.Author is null)
        {
            await SendSystemMessage(e.Message);
        }
        else
        {
            await SendUserMessage(e.Message);
        }
    }

    private async Task SendChatsInfoToUnauthenticatedUser(string connectionId, IEnumerable<Chat> chats, string? search)
    {
        IEnumerable<Chat> chatsInfo = new List<Chat>(chats);

        if (string.IsNullOrWhiteSpace(search) == false)
        {
            chatsInfo = chatsInfo.Where(chat => chat.Name.ToLower().Contains(search.ToLower().Trim()));
        }

        await SendChatsInfo(connectionId, chatsInfo.Select(chat => chat.ToDTO()));
    }
    private async Task SendChatsInfoToAuthenticatedUser(string connectionId, string userName, IEnumerable<Chat> chats, string? search)
    {
        await SendOwnChatsInfo(connectionId, chats.Where(chat => chat.Author.Equals(userName)).Select(chat => chat.ToDTO()));

        IEnumerable<Chat> chatsInfo = new List<Chat>(chats.Where(chat => chat.Author.Equals(userName) == false));

        if (string.IsNullOrWhiteSpace(search) == false)
        {
            chatsInfo = chatsInfo.Where(chat => chat.Name.ToLower().Contains(search.ToLower().Trim()));
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

    private async Task SendUserMessage(Message message)
    {
        await _hubContext.Clients.Group(message.Chat.Name).SendAsync(ChatHub.ReceiveUserMessageMethod, message.ToDTO());
    }
    private async Task SendSystemMessage(Message message)
    {
        await _hubContext.Clients.Group(message.Chat.Name).SendAsync(ChatHub.ReceiveSystemMessageMethod, message.ToDTO());
    }
}
