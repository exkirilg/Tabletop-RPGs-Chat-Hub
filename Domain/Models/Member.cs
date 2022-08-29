using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

[Index(nameof(MemberId), IsUnique = true)]
public class Member : IComparable<Member>
{
    public const int NameMinLength = 5;
    public const int NameMaxLength = 100;

    private string _name;

    [Key]
    public Guid MemberId { get; init; } = Guid.NewGuid();

    [Required]
    public Chat Chat { get; init; }

    [Required(AllowEmptyStrings = false)]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            _name = value.Trim();
        }
    }

    private Member()
    {
    }
    public Member(Chat chat, string name)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));

        Chat = chat;
        Name = name;
    }

    public int CompareTo(Member? other)
    {
        if (other is null) return 1;

        return Name.CompareTo(other.Name);
    }
}
