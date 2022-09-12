using Domain.Models;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class DiceRollRepository : Repository<DiceRoll>, IDiceRollRepository
{
	public DiceRollRepository(ChatHubContext context) : base(context)
	{
	}

    public override async Task<IEnumerable<DiceRoll>> GetAllAsync()
    {
        return await _context.DiceRolls
            .OrderBy(r => r.Position)
            .Include(r => r.Message)
            .ToListAsync();
    }
    public override async Task<DiceRoll> GetByIdAsync(Guid id)
    {
        var diceRoll = await _context.DiceRolls.FindAsync(id);

        if (diceRoll is null) throw new Exception($"There is no {nameof(diceRoll)} with id: {id}");
        
        await _context.Entry(diceRoll).Reference(nameof(diceRoll.Message)).LoadAsync();

        return diceRoll;
    }

    public override async Task<IEnumerable<DiceRoll>> GetAllByExpression(Expression<Func<DiceRoll, bool>> predicate)
    {
        return await _context.DiceRolls
            .Where(predicate)
            .Include(r => r.Message)
            .OrderBy(r => r.Position)
            .ToListAsync();
    }
    public override async Task<DiceRoll?> GetFirstOrDefaultByExpression(Expression<Func<DiceRoll, bool>> predicate)
    {
        return await _context.DiceRolls
            .Where(predicate)
            .Include(r => r.Message)
            .FirstOrDefaultAsync();
    }
}
