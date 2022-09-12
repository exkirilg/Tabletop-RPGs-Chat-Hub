using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess;

public class IdentityContext : IdentityDbContext
{
	public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
	{
	}
}
