using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoveMate.Models.Data;

namespace MoveMate.API.Database
{
    public interface IMoveMateDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutData> WorkoutData { get; set; }
        public DbSet<EnumEntity<WorkoutType>> WorkoutTypes { get; set; }


        public Task<int> SaveChangesAsync();
        public void Remove<TEntity>(TEntity entity) where TEntity : notnull;
    }
}
