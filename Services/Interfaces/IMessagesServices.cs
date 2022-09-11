using Domain.Models;

namespace Services.Interfaces;

public interface IMessagesServices
{
    Task<IEnumerable<Message>> GetMessagesOnDateAsync(DateOnly date);
    Task<Message> CreateNewSystemMessageAsync(Guid chatId, string text);
    Task<Message> CreateNewUserMessageAsync(Guid memberId, string text);
}
