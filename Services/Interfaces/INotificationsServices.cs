using Services.CustomEventsArguments;

namespace Services.Interfaces;

public interface INotificationsServices
{
    event EventHandler<StatisticsChangedEventArgs> StatisticsChanged;
    event EventHandler<ChatsChangedEventArgs> ChatsChanged;
    event EventHandler<MemberChangedEventArgs> MemberCreated;
    event EventHandler<MemberChangedEventArgs> MemberUpdated;
    event EventHandler<MemberChangedEventArgs> MemberRemoved;

    void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e);
    void InvokeChatsChanged(object sender, ChatsChangedEventArgs e);
    void InvokeMemberCreated(object sender, MemberChangedEventArgs e);
    void InvokeMemberUpdated(object sender, MemberChangedEventArgs e);
    void InvokeMemberRemoved(object sender, MemberChangedEventArgs e);
}
