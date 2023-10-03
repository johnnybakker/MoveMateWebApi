using Microsoft.EntityFrameworkCore;
using MoveMate.Models.Data;

namespace MoveMate.API.Repositories
{
    public interface IWorkoutRepository
    {
        public Task<Workout?> Create(int userId, WorkoutType workoutType);
        public Task<Workout?> Get(int id);
        public Task<IEnumerable<Workout>> GetByUserId(int id);
    }
}
