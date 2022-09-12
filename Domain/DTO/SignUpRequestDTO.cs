using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record struct SignUpRequestDTO
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    public string Name { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Password confirmation does not match")]
    public string PasswordConfirmation { get; init; }
}
