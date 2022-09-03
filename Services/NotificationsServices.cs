using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Services;

public class NotificationsServices : INotificationsServices
{
    public event EventHandler<StatisticsChangedEventArgs>? StatisticsChanged;
    public event EventHandler<ChatsChangedEventArgs>? ChatsChanged;

    public void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e)
    {
        StatisticsChanged?.Invoke(sender, e);
    }
    public void InvokeChatsChanged(object sender, ChatsChangedEventArgs e)
    {
        ChatsChanged?.Invoke(sender, e);
    }
}
