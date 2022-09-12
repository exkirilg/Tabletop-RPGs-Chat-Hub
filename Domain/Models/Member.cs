using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

[Index(nameof(MemberId), IsUnique = true)]
public class Member : IComparable<Member>
{
    [Key]
    public Guid MemberId { get; init; } = Guid.NewGuid();

    [Required]
    public Chat Chat { get; init; }

    [Required(AllowEmptyStrings = true)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Nickname { get; set; }

    private Member()
    {
    }
    public Member(Chat chat, string username, string nickname)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));

        Chat = chat;
        Username = username;
        Nickname = nickname;
    }

    public MemberDTO ToDTO()
    {
        return new MemberDTO(MemberId, Chat.ChatId, Username, Nickname);
    }

    public int CompareTo(Member? other)
    {
        if (other is null) return 1;

        return Nickname.CompareTo(other.Nickname);
    }
}
