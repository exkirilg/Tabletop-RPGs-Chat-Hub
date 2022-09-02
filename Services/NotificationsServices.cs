using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Services;

public class NotificationsServices : INotificationsServices
{
    public event EventHandler<StatisticsChangedEventArgs>? StatisticsChanged;

    public void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e)
    {
        StatisticsChanged?.Invoke(sender, e);
    }
}
