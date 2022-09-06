using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Hubs;
using Services.CustomExceptions;
using Services.Interfaces;

namespace Server.Controllers;

[Route("api/chats")]
[ApiController]
public class ChatsController : ControllerBase
{
    private const int defNumberOfChats = 12;

    private readonly IChatServices _services;

    public ChatsController(IChatServices services)
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
    [HttpGet()]
    public async Task<IActionResult> GetChats(
        [FromQuery] int numberOfChats = defNumberOfChats, [FromQuery] string? search = null)
    {
        return Ok((await _services.GetChatsAsync(numberOfChats, search)).Select(chat => chat.ToDTO()));
    }

    /// <summary>
    /// Returns list of owned chats
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="401"></response>
    [HttpGet("own")]
    [Authorize]
    public async Task<IActionResult> GetOwnChats()
    {
        return Ok((await _services.GetChatsByAuthorAsync(User.Identity!.Name!)).Select(chat => chat.ToDTO()));
    }

    /// <summary>
    /// Returns list of others chats
    /// </summary>
    /// <param name="numberOfChats"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="401"></response>
    [HttpGet("others")]
    [Authorize]
    public async Task<IActionResult> GetOthersChats(
        [FromQuery] int numberOfChats = defNumberOfChats, [FromQuery] string? search = null)
    {
        return Ok((await _services.GetChatsByOtherAuthorsAsync(User.Identity!.Name!, numberOfChats, search)).Select(chat => chat.ToDTO()));
    }

    /// <summary>
    /// Return chat information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">If chat with specified id doesn't exist</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        try
        {
            return Ok((await _services.GetChatAsync(id)).ToDTO());
        }
        catch
        {
            ModelState.AddModelError("Id", "Chat with specified id doesn't exist");
            return ValidationProblem();
        }
    }

    /// <summary>
    /// Creates new chat and returns it's values
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error or if chat with specified name already exists</response>
    /// <response code="401">If unauthorized</response>
    [HttpPost("new")]
    [Authorize]
    public async Task<IActionResult> CreateNewChat([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] NewChatRequestDTO request)
    {
        Chat chat;

        var numberOfOwnChats = await _services.GetNumberOfChatsByAuthorAsync(User.Identity!.Name!);
        if (numberOfOwnChats >= ChatHub.MaxNumberOfOwnChats)
        {
            ModelState.AddModelError("MaxNumberOfOwnChats", "Maximal number of owned chats will be exceeded");
            return ValidationProblem();
        }

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
