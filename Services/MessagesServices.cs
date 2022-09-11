using Domain.DataAccessInterfaces;
using Domain.Models;
using Services.CustomEventsArguments;
using Services.Interfaces;

namespace Services;

public class MessagesServices : IMessagesServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationsServices _notificationsServices;

    public MessagesServices(IUnitOfWork unitOfWork, INotificationsServices notificationsServices)
    {
        _unitOfWork = unitOfWork;
        _notificationsServices = notificationsServices;
    }

    public async Task<IEnumerable<Message>> GetMessagesOnDateAsync(DateOnly date)
    {
        return await _unitOfWork.MessageRepository.GetMessagesOnDate(date);
    }

    public async Task<Message> CreateNewSystemMessageAsync(Guid chatId, string text)
    {
        var chat = await _unitOfWork.ChatRepository.GetByIdAsync(chatId);
        if (chat is null) throw new Exception($"There is no chat with id {chatId}");

        var message = new Message(chat, text);

        await _unitOfWork.MessageRepository.AddAsync(message);
        await _unitOfWork.CompleteAsync();

        _notificationsServices.InvokeNewMessage(this, new NewMessageEventArgs(message));

        return message;
    }

    public async Task<Message> CreateNewUserMessageAsync(Guid memberId, string text)
    {
        var member = await _unitOfWork.MemberRepository.GetByIdAsync(memberId);
        if (member is null) throw new Exception($"There is no member with id {memberId}");

        var message = new Message(member.Chat, text, member);

        await _unitOfWork.MessageRepository.AddAsync(message);
        await _unitOfWork.CompleteAsync();

        _notificationsServices.InvokeNewMessage(this, new NewMessageEventArgs(message));

        return message;
    }
}
