using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public IdentityController(IConfiguration configuration,
        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
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

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !(await _signInManager.CheckPasswordSignInAsync(user, request.Password, false)).Succeeded)
        {
            ModelState.AddModelError("Authentication", "Incorrect email or password");
            return BadRequest(ModelState);
        }

        return Ok(new SignInResponseDTO(CreateAccessToken(GetUserClaims(user)), user.UserName));
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

        if ((await _userManager.FindByEmailAsync(request.Email)) is not null)
        {
            ModelState.AddModelError("Email", $"User with email {request.Email} already exists");
            return BadRequest(ModelState);
        }

        var user = new IdentityUser() { Email = request.Email, UserName = request.Name };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return Ok(new SignInResponseDTO(CreateAccessToken(GetUserClaims(user)), user.UserName));
        }

        return BadRequest(ModelState);
    }

    private IEnumerable<Claim> GetUserClaims(IdentityUser user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };
    }
    private string CreateAccessToken(IEnumerable<Claim> claims)
    {
        var handler = new JwtSecurityTokenHandler();

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Auth:JWTSecret"])),
            SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Auth:Issuer"],
            Audience = _configuration["Auth:Audience"],
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials
        };

        var token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }
}
