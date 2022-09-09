using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("chat/{chatId}")]
    public async Task<IActionResult> GetMembers(Guid chatId)
    {
        return Ok((await _services.GetChatMembersAsync(chatId)).Select(m => m.ToDTO()));
    }

    /// <summary>
    /// Returns members of user
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="401">If unauthorized</response>
    [HttpGet()]
    public async Task<IActionResult> GetUserMembers()
    {
        return Ok((await _services.GetUserMembers(User.Identity!.Name!)).Select(m => m.ToDTO()));
    }

    /// <summary>
    /// Returns member info
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet("{memberId}")]
    public async Task<IActionResult> GetMemberInfo(Guid memberId)
    {
        try
        {
            return Ok((await _services.GetMemberAsync(memberId)).ToDTO());
        }
        catch
        {
            ModelState.AddModelError("Id", "Member with specified id doesn't exist");
            return ValidationProblem();
        }        
    }

    /// <summary>
    /// Creates new member for specified chat
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error</response>
    [HttpPost("new")]
    public async Task<IActionResult> JoinChat([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] NewMemberRequestDTO request)
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
    [HttpPost("remove/{memberId}")]
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

    /// <summary>
    /// Updates specified member with User data
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error</response>
    /// <response code="401">If unauthorized</response>
    [HttpPost("update/{memberId}")]
    [Authorize]
    public async Task<IActionResult> UpdateMember(Guid memberId)
    {
        try
        {
            await _services.UpdateMembersUserAsync(memberId, User.Identity!.Name!);
        }
        catch
        {
            ModelState.AddModelError("Id", "Member with specified id doesn't exist");
            return ValidationProblem();
        }

        return Ok();
    }
}
