using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Models;
using MoveMateWebApi.Repositories;

namespace MoveMateWebApi.Database;

public class MoveMateDbContext : DbContext
{
	private readonly IConfiguration _configuration;

	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Session> Sessions { get; set; }
	public virtual DbSet<Subscription> Subscription { get; set; }

    public MoveMateDbContext(IConfiguration configuration, DbContextOptions<MoveMateDbContext> options) : base(options)
    {
		_configuration = configuration;
    }

	protected override void OnConfiguring(DbContextOptionsBuilder builder)
	{
		var connectionString = _configuration.GetConnectionString("DefaultConnection");
		builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Subscription>()
			.HasOne(u=> u.User)
			.WithMany(e => e.Subscriptions)
			.HasForeignKey(e => e.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Subscription>()
			.HasOne(e => e.ToUser)
			.WithMany(e => e.Subscribers)
			.HasForeignKey(e => e.ToUserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Session>()
			.HasQueryFilter(e => e.Expired == false)
			.HasOne(e => e.User)
			.WithMany(e => e.Sessions)
			.HasForeignKey(e => e.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}



	public void ExpireSession(Session session) {

	}

	public UserRepository GetUserRepository()
	{
		throw new NotImplementedException();
	}

	public SessionRepository GetSessionRepository()
	{
		throw new NotImplementedException();
	}

	public void AsyncSaveChanges()
	{
		throw new NotImplementedException();
	}
}