namespace Domain.DataAccessInterfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IChatRepository ChatRepository { get; }
    IMemberRepository MemberRepository { get; }
    IMessageRepository MessageRepository { get; }
    Task CompleteAsync();
}
