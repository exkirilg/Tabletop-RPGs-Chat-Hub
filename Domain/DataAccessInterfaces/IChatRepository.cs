namespace Domain.DataAccessInterfaces;

public interface IChatRepository : IRepository<Chat>
{
    public Task<int> GetNumberOfChatsAsync();
    public Task<int> GetNumberOfChatsByAuthorAsync(string author);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsAsync(int numberOfChats);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsWithNameSearchAsync(int numberOfChats, string search);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByAuthorAsync(string author);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsAsync(string author, int numberOfChats);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsWithNameSearchAsync(string author, int numberOfChats, string search);
    public Task<bool> ChatExistsAsync(string name);
}
