using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.CustomEventsArguments;
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

    private readonly INotificationsServices _notificationsServices;
    private readonly IStatisticsServices _statisticsServices;

    public IdentityServices(IConfiguration configuration,
        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        INotificationsServices notificationsServices, IStatisticsServices statisticsServices)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _notificationsServices = notificationsServices;
        _statisticsServices = statisticsServices;
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
                msg += (string.IsNullOrEmpty(msg) ? string.Empty : " ") + error.Description;
            }

            throw new SignUpException(string.IsNullOrEmpty(msg) ? "Wasn't able to sign up" : msg);
        }

        await OnUsersChanged();

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

    private async Task OnUsersChanged()
    {
        _notificationsServices.InvokeStatisticsChanged(
            this,
            new StatisticsChangedEventArgs(await _statisticsServices.GetStatistics()));
    }
}
