﻿using Domain.DataAccessInterfaces;
using Domain.Models;
using Services.Interfaces;

namespace Services;

public class MembersServices : IMembersServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationsServices _notificationsServices;

    public MembersServices(IUnitOfWork unitOfWork, INotificationsServices notificationsServices)
    {
        _unitOfWork = unitOfWork;
        _notificationsServices = notificationsServices;
    }

    public async Task<IEnumerable<Member>> GetChatMembersAsync(Guid chatId)
    {
        return await _unitOfWork.MemberRepository.GetChatMembers(chatId);
    }

    public async Task<Member> CreateNewChatMemberAsync(Guid chatId, string username, string nickname)
    {
        var chat = await _unitOfWork.ChatRepository.GetByIdAsync(chatId);

        if (chat is null)
        {
            throw new Exception($"There is no chat with id {chatId}");
        }

        var member = new Member(chat, username, nickname);

        await _unitOfWork.MemberRepository.AddAsync(member);
        await _unitOfWork.CompleteAsync();

        return member;
    }

    public async Task RemoveMemberAsync(Guid id)
    {
        await _unitOfWork.MemberRepository.RemoveByIdAsync(id);
        await _unitOfWork.CompleteAsync();
    }
}