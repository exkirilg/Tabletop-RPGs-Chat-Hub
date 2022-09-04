using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
	public ChatRepository(ChatHubContext context) : base(context)
	{
	}

    public async Task<int> GetNumberOfChatsAsync()
    {
        return await _context.Chats.CountAsync();
    }
    public async Task<int> GetNumberOfChatsByAuthorAsync(string author)
    {
        return await _context.Chats
            .Where(chat => chat.Author.Equals(author))
            .CountAsync();
    }

    public override async Task<IEnumerable<Chat>> GetAllAsync()
    {
        return await _context.Chats
            .OrderBy(chat => chat.Name)
            .ToListAsync();
    }
    public override async Task<IEnumerable<Chat>> GetAllByExpression(Expression<Func<Chat, bool>> predicate)
    {
        return await _context.Chats
            .Where(predicate)
            .OrderBy(chat => chat.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsAsync(int numberOfChats)
    {
        return await _context.Chats
            .OrderBy(chat => chat.Name)
            .Take(numberOfChats)
            .ToListAsync();
    }
    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsWithNameSearchAsync(int numberOfChats, string search)
    {
        return await _context.Chats
            .Where(c => c.Name.ToLower().Contains(search.ToLower().Trim()))
            .OrderBy(chat => chat.Name)
            .Take(numberOfChats)
            .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByAuthorAsync(string author)
    {
        return await _context.Chats
            .Where(chat => chat.Author.Equals(author))
            .OrderBy(chat => chat.Name)
            .ToListAsync();
    }
    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsAsync(string author, int numberOfChats)
    {
        return await _context.Chats
            .Where(chat => chat.Author.Equals(author) == false)
            .OrderBy(chat => chat.Name)
            .Take(numberOfChats)
            .ToListAsync();
    }
    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsByOtherAuthorsWithNameSearchAsync(string author, int numberOfChats, string search)
    {
        return await _context.Chats
            .Where(chat => chat.Author.Equals(author) == false)
            .Where(c => c.Name.ToLower().Contains(search.ToLower().Trim()))
            .OrderBy(chat => chat.Name)
            .Take(numberOfChats)
            .ToListAsync();
    }

    public async Task<bool> ChatExistsAsync(string name)
    {
        return await _context.Chats.AnyAsync(chat => chat.Name.Equals(name));
    }
}
