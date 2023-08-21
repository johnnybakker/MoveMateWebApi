using MoveMateWebApi.Database;
using MoveMateWebApi.Models.Data;

namespace MoveMateWebApi.Repositories {

	public class SessionRepository 
	{
		private readonly MoveMateDbContext _context;

		public SessionRepository(MoveMateDbContext context) {
			_context = context;
		}

		public async Task<Session?> RefreshSession(Session oldSession) {
			if(oldSession.Expired) return null;
			Console.WriteLine("Expire old session" + oldSession.Id);
			oldSession.Expired = true;
			_context.Update(oldSession);
			await _context.SaveChangesAsync();
			return await NewSession(oldSession.UserId);
		}

		public async Task<Session?> NewSession(int userId) {
			Session session = new Session(){ UserId = userId };
			await _context.Sessions.AddAsync(session);
			await _context.SaveChangesAsync();
			return session;
		}

		public async Task<Session?> GetSession(int id, int userId) {
			Session? session = await _context.Sessions.FindAsync(id);
			if(session == null) return null;
			if(session.UserId != userId) return null;
			return session;
		}

		public async Task ExpireSession(int id, int userId) {
			Session? session = await _context.Sessions.FindAsync(id);
			if(session == null) return;
			if(session.UserId != userId) return;
			session.Expired = true;
			await _context.SaveChangesAsync();
		}
	}


}