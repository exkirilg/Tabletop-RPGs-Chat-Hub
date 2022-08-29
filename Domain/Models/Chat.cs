using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

[Index(nameof(ChatId), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
public class Chat : IComparable<Chat>
{
    public const int NameMinLength = 5;
    public const int NameMaxLength = 100;

    private string _name;

    [Key]
    public Guid ChatId { get; init; } = Guid.NewGuid();

    [Required(AllowEmptyStrings = false)]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    public string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            _name = value.Trim();
        }
    }

    private Chat()
    {
    }
    public Chat(string name)
    {
        Name = name;
    }
    
    public ChatDTO ToDTO()
    {
        return new ChatDTO(ChatId, Name);
    }
    public int CompareTo(Chat? other)
    {
        if (other is null) return 1;

        return Name.CompareTo(other.Name);
    }
}
