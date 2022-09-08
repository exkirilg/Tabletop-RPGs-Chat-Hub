using Services.CustomEventsArguments;

namespace Services.Interfaces;

public interface INotificationsServices
{
    event EventHandler<StatisticsChangedEventArgs> StatisticsChanged;
    event EventHandler<ChatsChangedEventArgs> ChatsChanged;
    event EventHandler<MemberCreatedEventArgs> MemberCreated;
    event EventHandler<MemberRemovedEventArgs> MemberRemoved;

    void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e);
    void InvokeChatsChanged(object sender, ChatsChangedEventArgs e);
    void InvokeMemberCreated(object sender, MemberCreatedEventArgs e);
    void InvokeMemberRemoved(object sender, MemberRemovedEventArgs e);
}
