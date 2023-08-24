using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Models.Data;
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
			.HasQueryFilter(e => e.ExpirationDate > DateTime.UtcNow)
			.HasOne(e => e.User)
			.WithMany(e => e.Sessions)
			.HasForeignKey(e => e.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}
}