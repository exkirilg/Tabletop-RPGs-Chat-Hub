using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Services;

public class NotificationsServices : INotificationsServices
{
    public event EventHandler<StatisticsChangedEventArgs>? StatisticsChanged;
    public event EventHandler<ChatsChangedEventArgs>? ChatsChanged;
    public event EventHandler<MemberChangedEventArgs>? MemberCreated;
    public event EventHandler<MemberChangedEventArgs>? MemberUpdated;
    public event EventHandler<MemberChangedEventArgs>? MemberRemoved;

    public void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e)
    {
        StatisticsChanged?.Invoke(sender, e);
    }
    public void InvokeChatsChanged(object sender, ChatsChangedEventArgs e)
    {
        ChatsChanged?.Invoke(sender, e);
    }
    public void InvokeMemberCreated(object sender, MemberChangedEventArgs e)
    {
        MemberCreated?.Invoke(sender, e);
    }
    public void InvokeMemberUpdated(object sender, MemberChangedEventArgs e)
    {
        MemberUpdated?.Invoke(sender, e);
    }
    public void InvokeMemberRemoved(object sender, MemberChangedEventArgs e)
    {
        MemberRemoved?.Invoke(sender, e);
    }
}
