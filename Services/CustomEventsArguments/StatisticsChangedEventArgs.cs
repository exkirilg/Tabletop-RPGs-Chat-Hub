using Domain.DTO;

namespace Services.CustomEventsArguments;

public class StatisticsChangedEventArgs : EventArgs
{
    public StatisticsDTO StatisticsDTO { get; init; }

	public StatisticsChangedEventArgs(StatisticsDTO statisticsDTO)
	{
        StatisticsDTO = statisticsDTO;
    }
}
