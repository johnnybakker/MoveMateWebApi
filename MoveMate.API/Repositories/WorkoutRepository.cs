using Microsoft.EntityFrameworkCore;
using MoveMate.API.Database;
using MoveMate.Models.Data;
using MoveMate.API.Services;

namespace MoveMate.API.Repositories {

	public class WorkoutRepository : Repository, IWorkoutRepository
	{
		public WorkoutRepository(IMoveMateDbContext context) : base(context) {
	
		}

		public async Task<Workout?> Create(int userId, WorkoutType workoutType) 
		{	
			Workout workout = new Workout(){ 
				UserId = userId, 
				Data = new(),
				Type = workoutType,
				StartDate = DateTime.Now,
				EndDate = null
			};

			await _context.Workouts.AddAsync(workout);
			await _context.SaveChangesAsync();
			
			return workout;
		}

		public async Task<Workout?> Get(int id) {
			return await _context.Workouts.FirstOrDefaultAsync(w => w.Id == id);
		} 

		public async Task<IEnumerable<Workout>> GetByUserId(int id) {
			User? user= await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			
			if(user==null) return new List<Workout>();
 
			return await _context.Workouts
				.Where(w => w.UserId == id)
				.ToListAsync();
		} 
	}


}