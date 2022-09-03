using Services.CustomEventsArguments;

namespace Services.Interfaces;

public interface INotificationsServices
{
    event EventHandler<StatisticsChangedEventArgs> StatisticsChanged;
    event EventHandler<ChatsChangedEventArgs> ChatsChanged;

    void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e);
    void InvokeChatsChanged(object sender, ChatsChangedEventArgs e);
}
