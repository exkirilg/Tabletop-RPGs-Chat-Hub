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
        settings.ChatsSearch = search;

        await SendChatsInfoToCurrentConnection(numberOfChats, search);
    }

    [Authorize]
    public async Task OwnChatsInfoRequest()
    {
        await SendOwnChatsInfoToCurrentConnection();
    }

    [Authorize]
    public async Task OthersChatsInfoRequest(int numberOfChats, string? search = default)
    {
        var settings = _state.GetConnectionSettings(Context.ConnectionId);
        settings.NumberOfChats = numberOfChats;
        settings.ChatsSearch = search;

        await SendOthersChatsInfoToCurrentConnection(numberOfChats, search);
    }

    #endregion

    private async Task SendChatsInfoToCurrentConnection(int numberOfChats, string? search)
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendAsync(
                ReceiveChatsInfoMethod,
                (await _chatServices.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO())
            );
    }

    [Authorize]
    private async Task SendOwnChatsInfoToCurrentConnection()
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendAsync(
                ReceiveOwnChatsInfoMethod,
                (await _chatServices.GetChatsByAuthorAsync(Context.User!.Identity!.Name!)).Select(chat => chat.ToDTO())
            );
    }

    [Authorize]
    private async Task SendOthersChatsInfoToCurrentConnection(int numberOfChats, string? search)
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendAsync(
                ReceiveOthersChatsInfoMethod,
                (await _chatServices.GetChatsByOtherAuthorsAsync(Context.User!.Identity!.Name!, numberOfChats, search)).Select(chat => chat.ToDTO())
            );
    }
}
