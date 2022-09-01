using Domain.DTO;

namespace Services.Interfaces;

public interface IStatisticsServices
{
    Task<StatisticsDTO> GetStatistics();
}
