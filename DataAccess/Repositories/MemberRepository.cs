		using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
	public MemberRepository(ChatHubContext context) : base(context)
	{
	}

	public override async Task<IEnumerable<Member>> GetAllAsync()
	{
		return await _context.Members
			.Include(m => m.Chat)
			.OrderBy(m => m)
			.ToListAsync();
	}
	public override async Task<Member> GetByIdAsync(Guid id)
	{
        var member = await _context.Members.FindAsync(id);

        if (member is null)
        {
            throw new Exception($"There is no {nameof(member)} with id: {id}");
        }

		await _context.Entry(member).Reference(nameof(member.Chat)).LoadAsync();

        return member;
    }
    public async Task<IEnumerable<Member>> GetChatMembers(Guid chatId)
    {
		return await _context.Members
			.Where(m => m.Chat.ChatId.Equals(chatId))
            .Include(m => m.Chat)
            .OrderBy(m => m.Nickname)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Member>> GetAllByExpression(Expression<Func<Member, bool>> predicate)
	{
        return await _context.Members
			.Where(predicate)
			.OrderBy(m => m)
			.Include(m => m.Chat)
			.ToListAsync();
    }
	public override async Task<Member?> GetFirstOrDefaultByExpression(Expression<Func<Member, bool>> predicate)
	{
        return await _context.Members
			.Where(predicate)
			.Include(m => m.Chat)
			.FirstOrDefaultAsync();
    }
}
