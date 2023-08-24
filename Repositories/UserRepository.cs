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
			.Include(u => u.Subscriptions)
			.Include(u => u.Sessions);
	}

	public async Task<User?> Get(int id) 
	{
		return await _dbContext.Users.FindAsync(id);
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
			Token = await _tokenService.Create(session)
		};

		return loginResult;
	}
}