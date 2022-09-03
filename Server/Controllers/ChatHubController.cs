using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<IdentityUser> _userManager;

    public ChatHubController(IChatServices services, UserManager<IdentityUser> userManager)
    {
        _services = services;
        _userManager = userManager;
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
    /// <response code="401">If unauthorized</response>
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> CreateNewChat([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] NewChatRequestDTO request)
    {
        Chat chat;

        try
        {
            chat = await _services.CreateNewChatAsync(request.Name, User.Identity!.Name!, request.Description);
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
