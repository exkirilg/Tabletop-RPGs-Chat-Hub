using Domain.Models;

namespace Services.Interfaces;

public interface IChatServices
{
    Task<int> GetNumberOfChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetAllChatsAsync();
    Task<IEnumerable<Chat>> GetChatsAsync(string? search);
    Task<IEnumerable<Chat>> GetChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetChatsByOtherAuthorsAsync(string author, string? search);
    Task<Chat> GetChatAsync(Guid id);
    Task<Chat> CreateNewChatAsync(string name, string author, string description);
    Task RemoveChatAsync(Guid chatId, string username);
}
