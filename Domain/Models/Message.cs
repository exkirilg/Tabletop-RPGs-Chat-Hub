using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

[Index(nameof(MessageId), IsUnique = true)]
public class Message : IComparable<Message>
{
    [Key]
    public Guid MessageId { get; init; } = Guid.NewGuid();

    [Required]
    public Chat Chat { get; init; }

    [Required]
    public Member Member { get; init; }

    [Required]
    public DateTime DateTimeCreated { get; init; } = DateTime.UtcNow;

    public string TextContent { get; set; }

    private Message()
    {
    }
    public Message(Chat chat, Member member, string textContent)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        ArgumentNullException.ThrowIfNull(member, nameof(member));

        if (string.IsNullOrWhiteSpace(textContent)) throw new ArgumentNullException(nameof(textContent));

        Chat = chat;
        Member = member;
        TextContent = textContent;
    }

    public int CompareTo(Message? other)
    {
        if (other is null) return 1;

        return DateTimeCreated.CompareTo(other.DateTimeCreated);
    }
}
