using System.Linq.Expressions;

namespace Domain.DataAccessInterfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllByExpression(Expression<Func<T, bool>> predicate);
    Task<T?> GetFirstOrDefaultByExpression(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    Task RemoveByIdAsync(Guid id);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
