using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Database;

public class MoveMateDbContext : DbContext
{
    public MoveMateDbContext(DbContextOptions<MoveMateDbContext> options) : base(options)
    {

    }

    public virtual DbSet<User> Users { get; set; }
}