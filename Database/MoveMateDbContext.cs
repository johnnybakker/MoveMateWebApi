using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Models.Data;
using MoveMateWebApi.Repositories;

namespace MoveMateWebApi.Database;

public class MoveMateDbContext : DbContext
{
	private readonly IConfiguration _configuration;
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Session> Sessions { get; set; }
	public virtual DbSet<Subscription> Subscriptions { get; set; }
	public virtual DbSet<Workout> Workouts { get; set; }
	public virtual DbSet<WorkoutData> WorkoutData { get; set; }
	public virtual DbSet<EnumEntity<WorkoutType>> WorkoutTypes { get; set; }

    public MoveMateDbContext(IConfiguration configuration, DbContextOptions<MoveMateDbContext> options) : base(options)
    {
		_configuration = configuration;
    }

	protected override void OnConfiguring(DbContextOptionsBuilder builder)
	{
		var connectionString = _configuration.GetConnectionString("DefaultConnection");
		builder
			.EnableThreadSafetyChecks()
			.UseLazyLoadingProxies(true)
			.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<EnumEntity<WorkoutType>>()
			.HasData(EnumEntity<WorkoutType>.Entities);

		modelBuilder.Entity<WorkoutData>().Property(x => x.Data).HasConversion(
        	v => JsonSerializer.Serialize<JsonObject>(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<JsonObject>(v, JsonSerializerOptions.Default)!
		);

		modelBuilder.Entity<Workout>()
			.HasMany(e => e.Data)
			.WithOne(e => e.Workout)
			.HasForeignKey(e => e.WorkoutId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Workout>()
			.HasOne(e => e.TypeEntity)
			.WithMany()
			.HasForeignKey(e =>e.TypeId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Workout>()
			.HasOne(e => e.User)
			.WithMany(e => e.Workouts)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Subscription>()
			.HasOne(u=> u.User)
			.WithMany(e => e.Subscriptions)
			.HasForeignKey(u=>u.UserId)
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
			.HasForeignKey(e=>e.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}
}