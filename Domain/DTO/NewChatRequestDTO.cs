using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record struct NewChatRequestDTO
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(Chat.NameMaxLength)]
    [MinLength(Chat.NameMinLength)]
    public string Name { get; init; }
}
