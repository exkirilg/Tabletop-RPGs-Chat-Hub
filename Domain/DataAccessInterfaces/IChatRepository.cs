namespace Domain.DataAccessInterfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<int> GetNumberOfChatsAsync();
    Task<int> GetNumberOfChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetSpecificNumberOfChatsAsync(int numberOfChats);
    Task<IEnumerable<Chat>> GetSpecificNumberOfChatsWithNameSearchAsync(int numberOfChats, string search);
    Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByAuthorAsync(string author);
    Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsAsync(string author, int numberOfChats);
    Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsWithNameSearchAsync(string author, int numberOfChats, string search);
    Task<bool> ChatExistsAsync(string name);
}
