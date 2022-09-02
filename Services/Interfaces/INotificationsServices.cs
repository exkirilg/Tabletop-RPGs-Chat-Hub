using Services.CustomEventsArguments;

namespace Services.Interfaces;

public interface INotificationsServices
{
    event EventHandler<StatisticsChangedEventArgs> StatisticsChanged;

    void InvokeStatisticsChanged(object sender, StatisticsChangedEventArgs e);
}
