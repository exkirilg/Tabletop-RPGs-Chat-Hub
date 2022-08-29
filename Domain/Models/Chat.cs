using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

[Index(nameof(ChatId), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
public class Chat : IComparable<Chat>
{
    private string _name;

    [Key]
    public Guid ChatId { get; init; } = Guid.NewGuid();

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 symbols")]
    [MinLength(10, ErrorMessage = "Name must be at least 10 symbols long")]
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
