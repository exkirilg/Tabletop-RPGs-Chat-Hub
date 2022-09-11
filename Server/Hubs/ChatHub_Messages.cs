namespace Server.Hubs;

public partial class ChatHub
{
    public const string ReceiveMessagesMethod = "ReceiveMessages";
    public const string ReceiveSystemMessageMethod = "ReceiveSystemMessage";
    public const string ReceiveUserMessageMethod = "ReceiveUserMessage";

    public async Task SendMessageRequest(Guid memberId, string text)
    {
        await _messagesServices.CreateNewUserMessageAsync(memberId, text);
    }
}
