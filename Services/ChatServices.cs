using Domain.DataAccessInterfaces;
using Domain.Models;
using Services.CustomExceptions;
using Services.Interfaces;

namespace Services;

public class ChatServices : IChatServices
{
    private readonly IUnitOfWork _unitOfWork;

	public ChatServices(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

    public async Task<int> GetNumberOfChatsAsync()
    {
        return await _unitOfWork.ChatRepository.GetNumberOfChats();
    }

    public async Task<IEnumerable<Chat>> GetChatsAsync(int numberOfChats, string? search)
	{
        if (search is not null)
        {
            return await _unitOfWork.ChatRepository.GetSpecificNumberOfChatsWithNameSearchAsync(numberOfChats, search);
        }
        
        return await _unitOfWork.ChatRepository.GetSpecificNumberOfChatsAsync(numberOfChats);
    }

    public async Task<Chat> CreateNewChatAsync(string chatName)
    {
        if (await _unitOfWork.ChatRepository.ChatExistsAsync(chatName))
        {
            throw new ChatAlreadyExistsException($"Chat with name {chatName} already exists");
        }

        Chat chat = new(chatName);

        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.CompleteAsync();

        return chat;
    }

    public async Task RemoveChatAsync(Guid chatId)
    {
        await _unitOfWork.ChatRepository.RemoveByIdAsync(chatId);
    }
}
