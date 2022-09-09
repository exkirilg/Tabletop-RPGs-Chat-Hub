using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;

namespace Server.Hubs;

public partial class ChatHub : Hub
{
    public const int MaxNumberOfOwnChats = 4;

    private readonly IStatisticsServices _statisticsServices;
    private readonly IChatServices _chatServices;
    private readonly IMembersServices _membersServices;

    private readonly ChatHubBroadcast _broadcast;
    private readonly State _state;

    public ChatHub(
        IStatisticsServices statisticsServices, IChatServices chatServices, IMembersServices membersServices,
        ChatHubBroadcast broadcast, State state)
    {
        _statisticsServices = statisticsServices;
        _chatServices = chatServices;
        _membersServices = membersServices;
        _broadcast = broadcast;
        _state = state;
    }

    public override async Task OnConnectedAsync()
    {
        _state.AddConnectionSettings(Context.ConnectionId);

        if (Context.User?.Identity?.Name is not null)
        {
            _state.GetConnectionSettings(Context.ConnectionId).UserName = Context.User.Identity.Name;
        }

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var settings = _state.GetConnectionSettings(Context.ConnectionId);

        if (settings.UserName is null)
        {
            foreach (var member in settings.GetMembers())
            {
                await _membersServices.RemoveMemberAsync(member.MemberId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, member.Chat.Name);
            }
        }

        _state.RemoveConnectionSettings(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
