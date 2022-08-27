using DataAccess.Repositories;

namespace DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChatHubContext _context;

    public IChatRepository ChatRepository { get; }
    public IMemberRepository MemberRepository { get; }
    public IMessageRepository MessageRepository { get; }

    public UnitOfWork(ChatHubContext context)
	{
		_context = context;

        ChatRepository = new ChatRepository(_context);
        MemberRepository = new MemberRepository(_context);
        MessageRepository = new MessageRepository(_context);
    }

	public async Task CompleteAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async ValueTask DisposeAsync()
	{
		await _context.DisposeAsync();
		GC.SuppressFinalize(this);
	}
}
