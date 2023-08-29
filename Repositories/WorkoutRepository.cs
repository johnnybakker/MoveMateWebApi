using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models.Data;
using MoveMateWebApi.Services;

namespace MoveMateWebApi.Repositories {

	public class WorkoutRepository 
	{
		private readonly MoveMateDbContext _context;
		private readonly FireBaseService _firebase;

		public WorkoutRepository(MoveMateDbContext context, FireBaseService firebase) {
			_context = context;
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
				.Where(w => w.UserId == id || user.SubscriptionIds.Contains(w.UserId))
				.ToListAsync();
		} 
	}


}