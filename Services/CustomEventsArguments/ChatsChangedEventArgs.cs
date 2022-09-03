using Domain.Models;

namespace Services.CustomEventsArguments;

public class ChatsChangedEventArgs : EventArgs
{
    public IEnumerable<Chat> Chats { get; init; }

    public ChatsChangedEventArgs(IEnumerable<Chat> chats)
    {
        Chats = chats;
    }
}
