﻿namespace Domain.Models;

public class Member
{
    private string _name;

    public Guid MemberId { get; init; } = Guid.NewGuid();
    public Chat Chat { get; init; }
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _name = value;
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
}
