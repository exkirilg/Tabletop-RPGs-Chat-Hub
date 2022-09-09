using Domain.DataAccessInterfaces;
using Domain.Models;
using Services.CustomEventsArguments;
using Services.CustomExceptions;
using Services.Interfaces;

namespace Services;

public class ChatServices : IChatServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationsServices _notificationsServices;
    private readonly IStatisticsServices _statisticsServices;

    public ChatServices(IUnitOfWork unitOfWork, INotificationsServices notificationsServices, IStatisticsServices statisticsServices)
	{
		_unitOfWork = unitOfWork;
        _notificationsServices = notificationsServices;
        _statisticsServices = statisticsServices;
    }

    public async Task<int> GetNumberOfChatsByAuthorAsync(string author)
    {
        return await _unitOfWork.ChatRepository.GetNumberOfChatsByAuthorAsync(author);
    }

    public async Task<IEnumerable<Chat>> GetAllChatsAsync()
    {
        return await _unitOfWork.ChatRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Chat>> GetChatsAsync(string? search)
	{
        if (search is not null)
        {
            return await _unitOfWork.ChatRepository.GetChatsWithNameSearchAsync(search);
        }
        
        return await _unitOfWork.ChatRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Chat>> GetChatsByAuthorAsync(string author)
    {
        return await _unitOfWork.ChatRepository.GetChatsByAuthorAsync(author);
    }

    public async Task<IEnumerable<Chat>> GetChatsByOtherAuthorsAsync(string author, string? search)
    {
        if (search is not null)
        {
            return await _unitOfWork.ChatRepository.GetChatsByOtherAuthorsWithNameSearchAsync(author, search);
        }

        return await _unitOfWork.ChatRepository.GetChatsByOtherAuthorsAsync(author);
    }

    public async Task<Chat> GetChatAsync(Guid id)
    {
        return await _unitOfWork.ChatRepository.GetByIdAsync(id);
    }

    public async Task<Chat> CreateNewChatAsync(string name, string author, string description)
    {
        if (await _unitOfWork.ChatRepository.ChatExistsAsync(name))
        {
            throw new ChatAlreadyExistsException($"Chat with name {name} already exists");
        }

        Chat chat = new(name, author);
        if (string.IsNullOrWhiteSpace(description) == false)
        {
            chat.Description = description;
        }

        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.CompleteAsync();
        
        await OnChatsChanged();

        return chat;
    }

    public async Task RemoveChatAsync(Guid chatId)
    {
        await _unitOfWork.ChatRepository.RemoveByIdAsync(chatId);

        await OnChatsChanged();
    }

    private async Task OnChatsChanged()
    {
        _notificationsServices.InvokeStatisticsChanged(
            this,
            new StatisticsChangedEventArgs(await _statisticsServices.GetStatistics()));

        _notificationsServices.InvokeChatsChanged(
            this,
            new ChatsChangedEventArgs(await GetAllChatsAsync()));
    }
}
