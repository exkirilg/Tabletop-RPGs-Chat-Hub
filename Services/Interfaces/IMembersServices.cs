using Domain.Models;

namespace Services.Interfaces;

public interface IMembersServices
{
    Task<IEnumerable<Member>> GetChatMembersAsync(Guid chatId);
    Task<Member> GetMemberAsync(Guid id);
    Task<IEnumerable<Member>> GetUserMembers(string username);
    Task<Member> CreateNewChatMemberAsync(Guid chatId, string username, string nickname);
    Task UpdateMembersUserAsync(Guid id, string username);
    Task RemoveMemberAsync(Guid id);
}
