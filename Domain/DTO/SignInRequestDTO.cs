using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record struct SignInRequestDTO
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; init; }
}
