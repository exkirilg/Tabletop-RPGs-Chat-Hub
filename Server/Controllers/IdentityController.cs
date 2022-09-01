using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityServices _identityServices;

    public IdentityController(IIdentityServices identityServices)
    {
        _identityServices = identityServices;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error or if email or password are incorrect</response>
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInRequestDTO request)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        SignInResponseDTO result;

        try
        {
            result = await _identityServices.SignInAsync(request);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Authentication", ex.Message);
            return ValidationProblem();
        }

        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">In case of validation error or if user with specified email already exists</response>
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpRequestDTO request)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        SignInResponseDTO result;

        try
        {
            result = await _identityServices.SignUpAsync(request);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Authentication", ex.Message);
            return ValidationProblem();
        }

        return Ok(result);
    }
}
