namespace Domain.Models;

public class Message
{
    public Guid MessageId { get; } = Guid.NewGuid();
    public Chat Chat { get; init; }
    public Member Member { get; init; }
    public DateTime DateTimeCreated { get; } = DateTime.UtcNow;
    public string TextContent { get; set; }

    public Message(Chat chat, Member member, string textContent)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        ArgumentNullException.ThrowIfNull(member, nameof(member));

        if (string.IsNullOrWhiteSpace(textContent))
        {
            throw new ArgumentNullException(nameof(textContent));
        }

        Chat = chat;
        Member = member;
        TextContent = textContent;
    }
}
