using Domain.Models;

namespace Services.Interfaces;

public interface IMembersServices
{
    Task<IEnumerable<Member>> GetChatMembersAsync(Guid chatId);
    Task<Member> CreateNewChatMemberAsync(Guid chatId, string username, string nickname);
    Task RemoveMemberAsync(Guid id);
}
