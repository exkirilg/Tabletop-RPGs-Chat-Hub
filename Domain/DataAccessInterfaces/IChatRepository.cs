namespace Domain.DataAccessInterfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<int> GetNumberOfChatsAsync();
    Task<int> GetNumberOfChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetChatsWithNameSearchAsync(string search);
    Task<IEnumerable<Chat>> GetChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetChatsByOtherAuthorsAsync(string author);
    Task<IEnumerable<Chat>> GetChatsByOtherAuthorsWithNameSearchAsync(string author, string search);
    Task<bool> ChatExistsAsync(string name);
}
