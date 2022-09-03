﻿using Domain.DataAccessInterfaces;
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

    public async Task<IEnumerable<Chat>> GetAllChatsAsync()
    {
        return await _unitOfWork.ChatRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Chat>> GetChatsAsync(int numberOfChats, string? search)
	{
        if (search is not null)
        {
            return await _unitOfWork.ChatRepository.GetSpecificNumberOfChatsWithNameSearchAsync(numberOfChats, search);
        }
        
        return await _unitOfWork.ChatRepository.GetSpecificNumberOfChatsAsync(numberOfChats);
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
