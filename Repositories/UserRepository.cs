using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models.Data;
using MoveMateWebApi.Models.Dto;
using MoveMateWebApi.Services;

namespace MoveMateWebApi.Repositories;



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
			.Include(u => u.Subscriptions);
	}

	public async Task<User?> Get(int id) 
	{
		return await _dbContext.Users.FindAsync(id);
	}

	public async Task<IEnumerable<int>> GetSubscriptions(int id) {
		return await _dbContext.Subscriptions
			.Where(e => e.UserId == id)
			.Select(e => e.ToUserId)
			.ToListAsync();
	}

	public async Task<IEnumerable<int>> GetSubscribers(int id) {
		return await _dbContext.Subscriptions
			.Where(e => e.ToUserId == id)
			.Select(e => e.UserId)
			.ToListAsync();
	}

	public async Task<IEnumerable<string>> GetSubscriberFirebaseTokens(int id) {
		IEnumerable<int> subscribers = await GetSubscribers(id);
		if(subscribers.Count() == 0) {
			Console.WriteLine($"Subscriber count for {id} is 0");
			return new List<string>();
		}
		return await _dbContext.Sessions
			.Where(e => subscribers.Contains(e.UserId) && !string.IsNullOrEmpty(e.FirebaseToken))
			.Select(s => s.FirebaseToken!)
			.ToListAsync();
	}

	async public Task<bool> Subscribe(int subscriberId, int subscriptionId)
	{
		if(await GetBySubscriberAndSubscription(subscriberId, subscriptionId) != null) 
			return true;

		var subscriber = await _dbContext.Users.FindAsync(subscriberId);
		if(subscriber == null) return false;

		var subscription = await _dbContext.Users.FindAsync(subscriptionId);
		if(subscription == null) return false;

		await _dbContext.Subscriptions.AddAsync(
			new() {
				IsFavorite = false,
				User = subscriber,
				ToUser = subscription
			}
		);

		await _dbContext.SaveChangesAsync();
		return true;
	}

	async public Task<Subscription?> GetBySubscriberAndSubscription(int subscriberId, int subscriptionId) {
		var result = await _dbContext.Subscriptions.FirstOrDefaultAsync(e => e.UserId == subscriberId && e.ToUserId == subscriptionId);
		return result;
	}

	async public Task<bool> UnSubscribe(int subscriberId, int subscriptionId)
	{
		var result = await GetBySubscriberAndSubscription(subscriberId, subscriptionId);	
		if(result == null) return true;

		_dbContext.Remove(result);
		await _dbContext.SaveChangesAsync();

		return true;
	}

	public IEnumerable<User> Search(string username){
		return _dbContext.Users
			.Include(x => x.Subscribers)
			.Include(x => x.Subscriptions)
			.Where(u => u.Username.ToLower().Contains(username.ToLower()))
			.OrderByDescending(u => u.Username.ToLower().StartsWith(username.ToLower()));
	}
	
	public async Task<SignUpResult> SignUp(SignUpRequest request) {

		SignUpResult result = new SignUpResult();
		result.IsValidEmail = request.Email.IsValidEmailAddress();
		result.IsStrongPassword = request.Password.IsStrongPassword();

		var users = _dbContext.Users
			.Where(u => u.Email == request.Email || u.Username == request.Username)
			.ToList();

		result.UserNameAlreadyExists = users.Where(u => u.Username == request.Username).Count() > 0;
		result.EmailAlreadyExists = users.Where(u => u.Email == request.Email).Count() > 0;

		if(result.IsValid) 
		{
			await _dbContext.Users.AddAsync(new() {
				Username = request.Username,
				Email = request.Email,
				Password = request.Password.ToSHA256HashedString()
			});

			await _dbContext.SaveChangesAsync();
		}

		return result;
	}

	public async Task<LoginResult?> Login(LoginRequest request) {
		string email = request.Email ?? "";
		string password = request.Password ?? "";
		string hashedPassword = password.ToSHA256HashedString();

		User? user = _dbContext.Users
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