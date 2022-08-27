namespace Domain.Models;

public class Chat
{
    public Guid ChatId { get; } = Guid.NewGuid();
    public string Name { get; init; }

    public Chat(string name)
    {
        Name = name;
    }
}
