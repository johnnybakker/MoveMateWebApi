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
		//base.OnModelCreating(modelBuilder);

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
	}

	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Subscription> Subscription { get; set; }
}