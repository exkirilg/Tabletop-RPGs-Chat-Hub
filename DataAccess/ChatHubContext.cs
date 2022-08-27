namespace DataAccess;

public class ChatHubContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Message> Messages { get; set; }

    public ChatHubContext(DbContextOptions<ChatHubContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
