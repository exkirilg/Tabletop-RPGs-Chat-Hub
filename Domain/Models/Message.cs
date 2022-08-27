namespace Domain.Models;

public class Message
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public Chat Chat { get; init; }
    public Member Member { get; init; }
    public DateTime DateTimeCreated { get; init; } = DateTime.UtcNow;
    public string TextContent { get; set; }

    private Message()
    {
    }
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
