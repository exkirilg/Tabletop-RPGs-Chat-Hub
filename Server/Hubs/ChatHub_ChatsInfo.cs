using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public partial class ChatHub
{
    public const string ReceiveChatsInfoMethod = "ReceiveChatsInfo";
    public const string ReceiveOwnChatsInfoMethod = "ReceiveOwnChatsInfo";
    public const string ReceiveOthersChatsInfoMethod = "ReceiveOthersChatsInfo";

    #region Requests

    public async Task ChatsInfoRequest(int numberOfChats, string? search = default)
    {
        var settings = _state.GetConnectionSettings(Context.ConnectionId);
        settings.NumberOfChats = numberOfChats;
        settings.ChatSearch = search;

        await SendChatsInfo(Context.ConnectionId, numberOfChats, search);
    }

    [Authorize]
    public async Task OwnChatsInfoRequest()
    {
        await SendOwnChatsInfo(Context.ConnectionId, Context.User!.Identity!.Name!);
    }

    [Authorize]
    public async Task OthersChatsInfoRequest(int numberOfChats, string? search = default)
    {
        var settings = _state.GetConnectionSettings(Context.ConnectionId);
        settings.NumberOfChats = numberOfChats;
        settings.ChatSearch = search;

        await SendOthersChatsInfo(Context.ConnectionId, Context.User!.Identity!.Name!, numberOfChats, search);
    }

    #endregion

    private async Task SendChatsInfo(string connectionId, int numberOfChats, string? search)
    {
        await Clients
            .Client(connectionId)
            .SendAsync(
                ReceiveChatsInfoMethod,
                (await _chatServices.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO())
            );
    }
    private async Task SendOwnChatsInfo(string connectionId, string UserName)
    {
        await Clients
            .Client(connectionId)
            .SendAsync(
                ReceiveOwnChatsInfoMethod,
                (await _chatServices.GetChatsByAuthorAsync(UserName)).Select(chat => chat.ToDTO())
            );
    }
    private async Task SendOthersChatsInfo(string connectionId, string UserName, int numberOfChats, string? search)
    {
        await Clients
            .Client(connectionId)
            .SendAsync(
                ReceiveOthersChatsInfoMethod,
                (await _chatServices.GetChatsByOtherAuthorsAsync(UserName, numberOfChats, search)).Select(chat => chat.ToDTO())
            );
    }
}
