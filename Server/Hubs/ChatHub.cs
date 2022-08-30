using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;

namespace Server.Hubs;

public class ChatHub : Hub
{
    public const string ReceiveChatsInfoMethod = "ReceiveChatsInfo";

    private readonly IChatServices _chatServices;

    public ChatHub(IChatServices chatServices)
    {
        _chatServices = chatServices;
    }

    public async Task ChatsInfoRequest(int numberOfChats, string? search = default)
    {
        await SendChatsInfo(numberOfChats, search);
    }

    private async Task SendChatsInfo(int numberOfChats, string? search)
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendAsync(
                ReceiveChatsInfoMethod,
                (await _chatServices.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO())
            );
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
