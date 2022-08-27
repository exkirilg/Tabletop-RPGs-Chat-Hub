using Domain.DataAccessInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatHubController : ControllerBase
{
    private const int defNumberOfRooms = 12;

    private IUnitOfWork _unitOfWork;

    public ChatHubController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetChatRooms(
        [FromQuery] int numberOfRooms = defNumberOfRooms, [FromQuery] string? search = null)
    {
        throw new NotImplementedException();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewRoom(string roomName)
    {
        throw new NotImplementedException();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> EnterRoom(string roomName)
    {
        throw new NotImplementedException();
    }
}
