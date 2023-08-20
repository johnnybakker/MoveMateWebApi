using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Database;

public class MoveMateDbContext : DbContext
{
	private readonly IConfiguration _configuration;

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
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<User>()
			.HasMany(u=> u.Subscriptions)
			.WithOne(s => s.User)
			.IsRequired();

		modelBuilder.Entity<User>()
			.HasMany(e => e.Subscribers)
			.WithOne(e => e.ToUser)
			.IsRequired();
	}

	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Subscription> Subscription { get; set; }
}