using Services.CustomEventsArguments;

namespace Services.Interfaces;

public interface INotificationsServices
{
    event EventHandler<StatisticsChangedEventArgs> StatisticsChanged;
    event EventHandler<ChatsChangedEventArgs> ChatsChanged;
    event EventHandler<ChatRemovedEventArgs> ChatRemoved;
    event EventHandler<MemberChangedEventArgs> MemberCreated;
    event EventHandler<MemberChangedEventArgs> MemberUpdated;
    event EventHandler<MemberChangedEventArgs> MemberRemoved;
    event EventHandler<NewMessageEventArgs> NewMessage;

    void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e);
    void InvokeChatsChanged(object sender, ChatsChangedEventArgs e);
    void InvokeChatRemoved(object sender, ChatRemovedEventArgs e);
    void InvokeMemberCreated(object sender, MemberChangedEventArgs e);
    void InvokeMemberUpdated(object sender, MemberChangedEventArgs e);
    void InvokeMemberRemoved(object sender, MemberChangedEventArgs e);
    void InvokeNewMessage(object sender, NewMessageEventArgs e);
}
