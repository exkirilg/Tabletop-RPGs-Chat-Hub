﻿namespace DataAccess.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
	public ChatRepository(ChatHubContext context) : base(context)
	{
	}

    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsAsync(int numberOfChats)
    {
        return await _context.Chats
            .Take(numberOfChats)
            .ToListAsync();
    }
    public async Task<IEnumerable<Chat>> GetSpecificNumberOfChatsWithNameSearchAsync(int numberOfChats, string search)
    {
        return await _context.Chats
            .Where(c => c.Name.ToLower().Contains(search.ToLower().Trim()))
            .Take(numberOfChats)
            .ToListAsync();
    }
    public async Task<Chat?> GetFirstOrDefaultByNameAsync(string name)
    {
        return await _context.Chats
            .FirstOrDefaultAsync(c => c.Name.Equals(name));
    }
}
