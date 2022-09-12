using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TRPG.DiceRoller;
using TRPG.DiceRoller.Adapters;
using TRPG.DiceRoller.RollsResults;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Models;

[Index(nameof(MessageId), IsUnique = true)]
public class Message : IComparable<Message>
{
    [Key]
    public Guid MessageId { get; init; } = Guid.NewGuid();

    [Required]
    public Chat Chat { get; init; }

    public Member? Author { get; init; }

    [Required]
    public DateTime DateTimeCreated { get; init; } = DateTime.UtcNow;

    public string TextContent { get; init; }

    public List<DiceRoll> DicePoolRoll { get; init; } = new();

    private Message()
    {
    }
    public Message(Chat chat, string textContent, Member? author = null)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));

        if (string.IsNullOrWhiteSpace(textContent)) throw new ArgumentNullException(nameof(textContent));

        Chat = chat;
        TextContent = textContent;
        Author = author;

        if (textContent.StartsWith("/roll"))
        {
            var diceRoller = new DiceRoller(new RandomIntAdapter());
            DicePoolRoll = diceRoller.RollDicePoolByExpression(textContent).Results
                .Select(r => new DiceRoll(this, r.Id, $"d{r.Dice.NumberOfSides}", r.Value))
                .ToList();

            TextContent = string.Empty;
        }
    }

    public MessageDTO ToDTO()
    {
        return new MessageDTO(
            MessageId,
            Chat.ChatId,
            Author?.MemberId,
            Author?.Nickname,
            Author is null,
            DateTimeCreated,
            TextContent,
            DicePoolRoll.Select(d => d.ToDTO()).ToList());
    }

    public int CompareTo(Message? other)
    {
        if (other is null) return 1;

        return DateTimeCreated.CompareTo(other.DateTimeCreated);
    }
}
