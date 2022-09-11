using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public partial class ChatHub
{
    public const string ReceiveChatHasBeenRemovedMethod = "ReceiveChatHasBeenRemoved";

    #region Requests

    public async Task JoinChatRequest(Guid memberId)
    {
        var member = await _membersServices.GetMemberAsync(memberId);
        _state.GetConnectionSettings(Context.ConnectionId).AddMember(member);

        await Groups.AddToGroupAsync(Context.ConnectionId, member.Chat.Name);

        await Clients.Client(Context.ConnectionId).SendAsync(
            ReceiveMessagesMethod,
            member.Chat.ChatId,
            (await _messagesServices.GetLastMessagesUpToDateAsync(member.Chat.ChatId, DateTime.Now)).Select(msg => msg.ToDTO()));
    }

    public async Task LeaveChatRequest(Guid memberId)
    {
        var member = await _membersServices.GetMemberAsync(memberId);
        _state.GetConnectionSettings(Context.ConnectionId).RemoveMember(memberId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, member.Chat.Name);
    }

    #endregion
}
