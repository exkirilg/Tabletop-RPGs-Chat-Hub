using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ChatHubContext _context;

    public Repository(ChatHubContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity is null)
        {
            throw new Exception($"There is no {nameof(T)} with id: {id}");
        }

        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllByExpression(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }
    public virtual async Task<T?> GetFirstOrDefaultByExpression(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        _context.Set<T>().Remove(await GetByIdAsync(id));
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}
