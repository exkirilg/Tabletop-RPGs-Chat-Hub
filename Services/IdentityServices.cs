using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.CustomExceptions;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services;

public class IdentityServices : IIdentityServices
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public IdentityServices(IConfiguration configuration,
        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<int> GetNumberOfUsersAsync()
    {
        return await Task.Run(() => _userManager.Users.Count());
    }

    public async Task<SignInResponseDTO> SignInAsync(SignInRequestDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !(await _signInManager.CheckPasswordSignInAsync(user, request.Password, false)).Succeeded)
        {
            throw new SignInException("Incorrect email or password");
        }

        return new SignInResponseDTO(CreateAccessToken(GetUserClaims(user)), user.UserName);
    }

    public async Task<SignInResponseDTO> SignUpAsync(SignUpRequestDTO request)
    {
        if ((await _userManager.FindByEmailAsync(request.Email)) is not null)
        {
            throw new SignUpException($"User with email {request.Email} already exists");
        }

        var user = new IdentityUser() { Email = request.Email, UserName = request.Name };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded == false)
        {
            var msg = string.Empty;
            foreach (var error in result.Errors)
            {
                msg += $"\n{error.Description}";
            }

            throw new SignUpException($"Wasn't able to sign up:{msg}");
        }

        return new SignInResponseDTO(CreateAccessToken(GetUserClaims(user)), user.UserName);
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
