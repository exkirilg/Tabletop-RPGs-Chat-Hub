namespace Domain.DataAccessInterfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetLastMessagesUpToDateAsync(Guid ChatId, DateTime date);
}
