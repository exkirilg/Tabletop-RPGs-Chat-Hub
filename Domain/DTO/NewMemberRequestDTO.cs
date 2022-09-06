using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record struct NewMemberRequestDTO
{
    [Required]
    public Guid ChatId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Nickname { get; init; }
}
