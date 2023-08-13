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

	public virtual DbSet<User> Users { get; set; }
}