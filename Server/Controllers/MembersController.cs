using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Interfaces;

namespace Server.Controllers;

[Route("api/members")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMembersServices _services;

    public MembersController(IMembersServices services)
    {
        _services = services;
    }

    /// <summary>
    /// Returns members of the specified chat
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMembers(Guid chatId)
    {
        return Ok((await _services.GetChatMembersAsync(chatId)).Select(m => m.ToDTO()));
    }

    /// <summary>
    /// Creates new member for specified chat
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error</response>
    [HttpPost("new")]
    public async Task<IActionResult> EnterChat([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] NewMemberRequestDTO request)
    {
        Member member;

        string username = User.Identity?.Name is null ? string.Empty : User.Identity.Name;

        try
        {
            member = await _services.CreateNewChatMemberAsync(request.ChatId, username, request.Nickname);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Member", ex.Message);
            return ValidationProblem();
        }

        return Ok(member.ToDTO());
    }

    /// <summary>
    /// Removes specified member
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error</response>
    [HttpPost("leave-{memberId}")]
    public async Task<IActionResult> LeaveChat(Guid memberId)
    {
        try
        {
            await _services.RemoveMemberAsync(memberId);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Member", ex.Message);
            return ValidationProblem();
        }

        return Ok();
    }

}
