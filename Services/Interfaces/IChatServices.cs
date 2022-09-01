using Domain.Models;

namespace Services.Interfaces;

public interface IChatServices
{
    Task<IEnumerable<Chat>> GetChatsAsync(int numberOfChats, string? search);
    Task<Chat> CreateNewChatAsync(string chatName);
    Task RemoveChatAsync(Guid chatId);
}
