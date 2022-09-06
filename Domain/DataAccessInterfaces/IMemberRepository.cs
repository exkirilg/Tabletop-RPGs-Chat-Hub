namespace Domain.DataAccessInterfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<IEnumerable<Member>> GetChatMembers(Guid chatId);
}
