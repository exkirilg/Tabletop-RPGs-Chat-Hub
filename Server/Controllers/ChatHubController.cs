using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.CustomExceptions;
using Services.Interfaces;

namespace Server.Controllers;

[Route("api/chats")]
[ApiController]
public class ChatHubController : ControllerBase
{
    private const int defNumberOfChats = 12;

    private readonly IChatServices _services;

    public ChatHubController(IChatServices services)
    {
        _services = services;
    }

    /// <summary>
    /// Returns list of chats
    /// </summary>
    /// <param name="numberOfChats"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet("chats")]
    public async Task<IActionResult> GetChats(
        [FromQuery] int numberOfChats = defNumberOfChats, [FromQuery] string? search = null)
    {
        return Ok((await _services.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO()));
    }
        
    /// <summary>
    /// Creates new chat and returns it's value
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error or if chat with specified name already exists</response>
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewChat([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] NewChatRequestDTO request)
    {
        Chat chat;

        try
        {
            chat = await _services.CreateNewChatAsync(request.Name);
        }
        catch (ChatAlreadyExistsException ex)
        {
            ModelState.AddModelError(nameof(request.Name), ex.Message);
            return ValidationProblem();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Chat", ex.Message);
            return ValidationProblem();
        }

        return Ok(chat.ToDTO());
    }
}
