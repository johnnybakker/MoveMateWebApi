using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models;
using MoveMateWebApi.Services;

namespace MoveMateWebApi.Repositories {



	public class UserRepository {

		internal MoveMateDbContext _dbContext;

		private readonly ITokenFactory _tokenService;
		private readonly SessionRepository _sessionRepository;
	
		public UserRepository(MoveMateDbContext dbContext, ITokenFactory tokenService, SessionRepository sessionRepository) {
			_dbContext = dbContext;
			_tokenService = tokenService;
			_sessionRepository = sessionRepository;
		}

		public IEnumerable<User> GetAll()
		{
			return _dbContext.Users
				.Include(u => u.Subscribers)
				.Include(u => u.Subscriptions)
				.Include(u => u.Sessions);
		}

		async public Task<bool> Subscribe(int subscriberId, int subscriptionId)
		{
			var subscriber = await _dbContext.Users.FindAsync(subscriberId);
			if(subscriber == null) return false;

			var subscription = await _dbContext.Users.FindAsync(subscriptionId);
			if(subscription == null) return false;

			await _dbContext.Subscription.AddAsync(
				new() {
					IsFavorite = false,
					User = subscriber,
					ToUser = subscription
				}
			);

			await _dbContext.SaveChangesAsync();
			return true;
		}


		public IEnumerable<User> Search(string username){
			return _dbContext.Users
				.Where(u => u.Name.ToLower().Contains(username.ToLower()))
				.OrderByDescending(u => u.Name.ToLower().StartsWith(username.ToLower()));
		}
		
		public async Task<string?> SignUp(SignUpDto dto) {

			if(dto.Email.IsValidEmailAddress() == false) {
				return "Not a valid email address";
			}
			
			if(dto.Password.IsStrongPassword() == false) {
				return "Password is not strong enough";
			}
			
			var users = _dbContext.Users
				.Where(u => u.Email == dto.Email || u.Name == dto.Username)
				.ToList();

			if(users.Count == 0) {
				
				await _dbContext.Users.AddAsync(new() {
					Name = dto.Username,
					Email = dto.Email,
					Password = dto.Password.ToSHA256HashedString()
				});

				await _dbContext.SaveChangesAsync();
				return "OK";
			}

			if(users.Find(u => u.Email == dto.Email) != null) {
				return "Email address already in use";
			}

			return "Username already in use";
		}

		public async Task<string?> LoginUsingSession(Session oldSession) {
			var session = await _sessionRepository.RefreshSession(oldSession);
			if(session == null) return null;
			return await _tokenService.Create(session);
		}

		public async Task<string?> LoginUsingEmailAndPassword(string email, string password) {
			
			string hashedPassword = password.ToSHA256HashedString();

			User? user = _dbContext.Users
				.Where(u => u.Email == email && u.Password == hashedPassword)
				.FirstOrDefault();

			if(user == null) return null;
			
			var session = await _sessionRepository.NewSession(user.Id);
			if(session == null) return null;
			return await _tokenService.Create(session);
		}
	}
}