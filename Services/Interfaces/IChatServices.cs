using Domain.Models;

namespace Services.Interfaces;

public interface IChatServices
{
    Task<IEnumerable<Chat>> GetAllChatsAsync();
    Task<IEnumerable<Chat>> GetChatsAsync(int numberOfChats, string? search);
    Task<Chat> CreateNewChatAsync(string name, string author, string description);
    Task RemoveChatAsync(Guid chatId);
}
