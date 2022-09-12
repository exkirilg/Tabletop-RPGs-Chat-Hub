using Domain.Models;

namespace Services.CustomEventsArguments;

public class ChatRemovedEventArgs : EventArgs
{
    public Chat Chat { get; init; }

	public ChatRemovedEventArgs(Chat chat)
	{
		Chat = chat;
	}
}
