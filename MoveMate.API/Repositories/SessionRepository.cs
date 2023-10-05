using Microsoft.EntityFrameworkCore;
using MoveMate.API.Database;
using MoveMate.Models.Data;
using MoveMate.API.Services;

namespace MoveMate.API.Repositories {

	public class SessionRepository : Repository, ISessionRepository
	{
		private readonly ITokenFactory _tokenFactory;
		
		public SessionRepository(IMoveMateDbContext context, ITokenFactory tokenFactory) : base(context) {
			_tokenFactory = tokenFactory;
		}

		public async Task<string?> Refresh(int id, TimeSpan duration) 
		{
			Session? session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
			if(session == null) return null;

			// Add one some time to the session
			session.ExpirationDate = DateTime.UtcNow.Add(duration);

			// New token for session will expire old token
			string token = await _tokenFactory.Create(session);
			session.Token = token;
			
			// Return the new token for given session
			return token;
		}

		public async Task SetFirebaseToken(int id, string? token) 
		{
			Session? session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
			if(session == null) return;

			// Add one some time to the session
			session.FirebaseToken = token;
			await _context.SaveChangesAsync();
		}

		public async Task<Session?> New(int userId, TimeSpan duration) 
		{	
			// Create for user with id
			Session session = new Session(){ 
				UserId = userId, 
				ExpirationDate = DateTime.UtcNow.Add(duration),
				Token = null
			};

			// Save session to the database
			await _context.Sessions.AddAsync(session);
			await _context.SaveChangesAsync();
			
			// Write token into session
			session.Token = await _tokenFactory.Create(session);
			await _context.SaveChangesAsync();

			// Return session
			return session;
		}

		public async Task<Session?> Get(int id) 
		{
			return await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<ICollection<Session>> GetAll() 
		{
			return await _context.Sessions.ToListAsync();
		}

		public async Task<ICollection<Session>> GetAllWithFirebaseToken() 
		{
			return await _context.Sessions.Where(e => e.FirebaseToken != null).ToListAsync();
		}

		public async Task<ICollection<Session>> GetByUserId(int userId) 
		{
			return await _context.Sessions.Where(e => e.User.Id == userId).ToListAsync();
		}

		public async Task Expire(int id) 
		{
			Session? session = await _context.Sessions.FirstOrDefaultAsync(u => u.Id == id);
			if(session == null) return;
			session.ExpirationDate = DateTime.UtcNow;
			session.Token = null;
			await _context.SaveChangesAsync();
		}
	}


}