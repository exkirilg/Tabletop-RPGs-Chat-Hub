using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
	public MessageRepository(ChatHubContext context) : base(context)
	{
	}

    public async Task<IEnumerable<Message>> GetMessagesOnDate(DateOnly date)
    {
        return await _context.Messages
            .Where(m => m.DateTimeCreated.Date.Equals(date))
            .OrderBy(m => m.DateTimeCreated)
            .Include(m => m.Chat)
            .Include(m => m.Author)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages
            .OrderBy(m => m.DateTimeCreated)
            .Include(m => m.Chat)
            .Include(m => m.Author)
            .ToListAsync();
    }
    public override async Task<Message> GetByIdAsync(Guid id)
    {
        var msg = await _context.Messages.FindAsync(id);

        if (msg is null)
        {
            throw new Exception($"There is no {nameof(msg)} with id: {id}");
        }

        await _context.Entry(msg).Reference(nameof(msg.Chat)).LoadAsync();
        await _context.Entry(msg).Reference(nameof(msg.Author)).LoadAsync();

        return msg;
    }

    public override async Task<IEnumerable<Message>> GetAllByExpression(Expression<Func<Message, bool>> predicate)
    {
        return await _context.Messages
            .Where(predicate)
            .OrderBy(m => m.DateTimeCreated)
            .Include(m => m.Chat)
            .Include(m => m.Author)
            .ToListAsync();
    }
    public override async Task<Message?> GetFirstOrDefaultByExpression(Expression<Func<Message, bool>> predicate)
    {
        return await _context.Messages
            .Where(predicate)
            .Include(m => m.Chat)
            .Include(m => m.Author)
            .FirstOrDefaultAsync();
    }
}
