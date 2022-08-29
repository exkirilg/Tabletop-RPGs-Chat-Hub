namespace Domain.DataAccessInterfaces;

public interface IChatRepository : IRepository<Chat>
{
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsAsync(int numberOfChats);
    public Task<IEnumerable<Chat>> GetSpecificNumberOfChatsWithNameSearchAsync(int numberOfChats, string search);
    public Task<bool> ChatExistsAsync(string name);
}
