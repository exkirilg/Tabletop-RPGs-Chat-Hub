using Domain.DataAccessInterfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;

namespace Services;

public class StatisticsServices : IStatisticsServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public StatisticsServices(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<StatisticsDTO> GetStatistics()
    {
        return new(
            await _unitOfWork.ChatRepository.GetNumberOfChats(),
            await Task.Run(() => _userManager.Users.Count()));
    }
}
