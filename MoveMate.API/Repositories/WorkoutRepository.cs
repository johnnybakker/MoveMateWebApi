using Microsoft.EntityFrameworkCore;
using MoveMate.Database;
using MoveMate.Models.Data;
using MoveMate.Services;

namespace MoveMate.Repositories {

	public class WorkoutRepository : Repository 
	{
		private readonly FireBaseService _firebase;

		public WorkoutRepository(MoveMateDbContext context, FireBaseService firebase) : base(context) {
			_firebase = firebase;
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
			return await _context.Workouts.FindAsync(id);
		} 

		public async Task<IEnumerable<Workout>> GetByUserId(int id) {
			User? user= await _context.Users.FindAsync(id);
			
			if(user==null) return new List<Workout>();
			
			await _context.Entry(user)
				.Reference(e => e.Subscriptions)
				.LoadAsync();
 
			return await _context.Workouts
				.Where(w => w.UserId == id)
				.ToListAsync();
		} 
	}


}