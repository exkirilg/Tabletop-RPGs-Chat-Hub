using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
	public MessageRepository(ChatHubContext context) : base(context)
	{
	}

    public override async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages.Include(m => m.Chat).Include(m => m.Member).ToListAsync();
    }
    public override async Task<Message> GetByIdAsync(Guid id)
    {
        var msg = await _context.Messages.FindAsync(id);

        if (msg is null)
        {
            throw new Exception($"There is no {nameof(msg)} with id: {id}");
        }

        await _context.Entry(msg).Reference(nameof(msg.Chat)).LoadAsync();
        await _context.Entry(msg).Reference(nameof(msg.Member)).LoadAsync();

        return msg;
    }

    public override async Task<IEnumerable<Message>> GetAllByExpression(Expression<Func<Message, bool>> predicate)
    {
        return await _context.Messages.Where(predicate).Include(m => m.Chat).Include(m => m.Member).ToListAsync();
    }
    public override async Task<Message?> GetFirstOrDefaultByExpression(Expression<Func<Message, bool>> predicate)
    {
        return await _context.Messages.Where(predicate).Include(m => m.Chat).Include(m => m.Member).FirstOrDefaultAsync();
    }
}
