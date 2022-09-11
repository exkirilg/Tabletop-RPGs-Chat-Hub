namespace Domain.DataAccessInterfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetMessagesOnDate(DateOnly date);
}
