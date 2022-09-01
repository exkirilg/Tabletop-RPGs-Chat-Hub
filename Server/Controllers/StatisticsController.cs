using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsServices _services;

    public StatisticsController(IStatisticsServices services)
    {
        _services = services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        return Ok(await _services.GetStatistics());
    }
}
