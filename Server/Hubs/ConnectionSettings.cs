using Domain.Models;

namespace Server.Hubs;

public class ConnectionSettings
{
    private List<Member> _members = new();

    public string? UserName { get; set; }
    public string? ChatSearch { get; set; }
    
    public void RemoveChat(Guid chatId)
    {
        _members = _members.Where(m => m.Chat.ChatId.Equals(chatId) == false).ToList();
    }
    public IEnumerable<Member> GetMembers()
    {
        return _members.ToList();
    }
    public void AddMember(Member member)
    {
        if (_members.Contains(member) == false)
        {
            _members.Add(member);
        }
    }
    public void RemoveMember(Guid memberId)
    {
        var member = _members.Where(m => m.MemberId.Equals(memberId)).FirstOrDefault();
        if (member is not null) _members.Remove(member);
    }
}
