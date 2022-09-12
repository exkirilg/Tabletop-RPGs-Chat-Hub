using Domain.Models;

namespace Services.CustomEventsArguments;

public class NewMessageEventArgs : EventArgs
{
    public Message Message { get; init; }

    public NewMessageEventArgs(Message message)
    {
        Message = message;
    }
}
