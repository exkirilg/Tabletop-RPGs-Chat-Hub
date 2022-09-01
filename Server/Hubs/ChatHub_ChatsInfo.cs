﻿using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public partial class ChatHub
{
    public const string ReceiveChatsInfoMethod = "ReceiveChatsInfo";

    public async Task ChatsInfoRequest(int numberOfChats, string? search = default)
    {
        await SendChatsInfoToCurrentConnection(numberOfChats, search);
    }

    private async Task SendChatsInfoToCurrentConnection(int numberOfChats, string? search)
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendAsync(
                ReceiveChatsInfoMethod,
                (await _chatServices.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO())
            );
    }
}
