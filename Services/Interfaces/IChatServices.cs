using Domain.Models;

namespace Services.Interfaces;

public interface IChatServices
{
    public Task<IEnumerable<Chat>> GetChatsAsync(int numberOfChats, string? search);
    public Task<Chat> CreateNewChatAsync(string chatName);
    public Task RemoveChatAsync(Guid chatId);
}
