using Microsoft.EntityFrameworkCore;
using MoveMate.API.Database;
using MoveMate.Models.Data;
using MoveMate.Models.Dto;
using MoveMate.API.Services;

namespace MoveMate.API.Repositories;



public class UserRepository : Repository, IUserRepository 
{
	private readonly ITokenFactory _tokenService;
	private readonly ISessionRepository _sessionRepository;

	public UserRepository(IMoveMateDbContext context, ITokenFactory tokenService, ISessionRepository sessionRepository) : base(context) {
		_tokenService = tokenService;
		_sessionRepository = sessionRepository;
	}

	public async Task<IEnumerable<User>> GetAll()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task<User?> Get(int id) 
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<IEnumerable<int>> GetSubscriptions(int id) {
		return await _context.Subscriptions
			.Where(e => e.User.Id == id)
			.Select(e => e.ToUser.Id)
			.ToListAsync();
	}

	public async Task<IEnumerable<int>> GetSubscribers(int id) {
		return await _context.Subscriptions
			.Where(e => e.ToUser.Id == id)
			.Select(e => e.User.Id)
			.ToListAsync();
	}

	public async Task<IEnumerable<string>> GetSubscriberFirebaseTokens(int id) {
		IEnumerable<int> subscribers = await GetSubscribers(id);
		if(subscribers.Count() == 0) {
			Console.WriteLine($"Subscriber count for {id} is 0");
			return new List<string>();
		}
		return await _context.Sessions
			.Where(e => subscribers.Contains(e.User.Id) && !string.IsNullOrEmpty(e.FirebaseToken))
			.Select(s => s.FirebaseToken!)
			.ToListAsync();
	}

	async public Task<bool> Subscribe(int subscriberId, int subscriptionId)
	{
		if(await GetBySubscriberAndSubscription(subscriberId, subscriptionId) != null) 
			return true;

		var subscriber = await _context.Users.FirstOrDefaultAsync(s => s.Id == subscriberId);
		if(subscriber == null) return false;

		var subscription = await _context.Users.FirstOrDefaultAsync(s => s.Id == subscriptionId);
		if(subscription == null) return false;

		await _context.Subscriptions.AddAsync(
			new() {
				IsFavorite = false,
				User = subscriber,
				ToUser = subscription
			}
		);

		await _context.SaveChangesAsync();
		return true;
	}

	async public Task<Subscription?> GetBySubscriberAndSubscription(int subscriberId, int subscriptionId) {
		var result = await _context.Subscriptions.FirstOrDefaultAsync(e => e.User.Id == subscriberId && e.ToUser.Id == subscriptionId);
		return result;
	}

	async public Task<bool> UnSubscribe(int subscriberId, int subscriptionId)
	{
		var result = await GetBySubscriberAndSubscription(subscriberId, subscriptionId);	
		if(result == null) return true;

		_context.Remove(result);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<IEnumerable<User>> Search(string username){
		return await _context.Users
			.Where(u => u.Username.ToLower().Contains(username.ToLower()))
			.OrderByDescending(u => u.Username.ToLower().StartsWith(username.ToLower()))
			.ToListAsync();
	}
	
	public async Task<SignUpResult> SignUp(SignUpRequest request) {

		SignUpResult result = new SignUpResult();
		result.IsValidEmail = request.Email.IsValidEmailAddress();
		result.IsStrongPassword = request.Password.IsStrongPassword();

		var users = _context.Users
			.Where(u => u.Email == request.Email || u.Username == request.Username)
			.ToList();

		result.UserNameAlreadyExists = users.Where(u => u.Username == request.Username).Count() > 0;
		result.EmailAlreadyExists = users.Where(u => u.Email == request.Email).Count() > 0;

		if(result.IsValid) 
		{
			await _context.Users.AddAsync(new() {
				Username = request.Username,
				Email = request.Email,
				Password = request.Password.ToSHA256HashedString()
			});

			await _context.SaveChangesAsync();
		}

		return result;
	}

	public async Task<LoginResult?> Login(LoginRequest request) {
		string email = request.Email ?? "";
		string password = request.Password ?? "";
		string hashedPassword = password.ToSHA256HashedString();

		User? user = _context.Users
			.Where(u => u.Email == email && u.Password == hashedPassword)
			.FirstOrDefault();

		if(user == null) return null;
		
		var session = await _sessionRepository.New(user.Id, TimeSpan.FromHours(1));
		if(session == null) return null;

		LoginResult loginResult = new LoginResult {
			Id = user.Id,
			Username = user.Username,
			Email = user.Email,
			Token = await _tokenService.Create(session),
			Subscribers = await GetSubscribers(user.Id),
			Subscriptions = await GetSubscriptions(user.Id)
		};

		return loginResult;
	}
}