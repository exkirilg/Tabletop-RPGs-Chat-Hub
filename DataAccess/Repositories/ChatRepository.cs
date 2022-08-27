namespace DataAccess.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
	public ChatRepository(ChatHubContext context) : base(context)
	{
	}
}
