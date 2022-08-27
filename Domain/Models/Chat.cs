namespace Domain.Models;

public class Chat
{
    public Guid ChatId { get; init; } = Guid.NewGuid();
    public string Name { get; init; }

    private Chat()
    {
    }
    public Chat(string name)
    {
        Name = name;
    }
}
