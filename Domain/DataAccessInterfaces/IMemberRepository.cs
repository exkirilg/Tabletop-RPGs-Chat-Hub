namespace Domain.DataAccessInterfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<IEnumerable<Member>> GetChatMembers(Guid chatId);
    Task<IEnumerable<Member>> GetUserMembers(string username);
    Task UpdateMembersUserAsync(Guid id, string username);
}
