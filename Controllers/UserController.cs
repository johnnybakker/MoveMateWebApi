using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models;
using MoveMateWebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;

namespace MoveMateWebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{
	protected readonly IConfiguration _config;
	protected readonly MoveMateDbContext _dbContext;
	
    public UserController(IConfiguration config, MoveMateDbContext dbContext)
    {
		_config = config;
		_dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
		return _dbContext.Users
			.Include(u => u.Subscribers)
			.Include(u => u.Subscriptions);
    }

	[HttpPost("[action]")]
    async public Task<string> Subscribe(JsonObject body)
    {
		int? id = body["id"]?.GetValue<int>();
		if(id == null) return "Id is not provided";

		int myUserId = User.GetUserId();
		var user = await _dbContext.Users.FindAsync(myUserId);

		if(user == null) {
			return $"User with id {myUserId} does not exist";
		}

		var toUser = await _dbContext.Users.FindAsync(id);
		if(toUser == null) {
			return $"User with id {id} does not exist";
		}

		await _dbContext.Subscription.AddAsync(
			new() {
				IsFavorite = false,
				ToUser = toUser,
				User = user
			}
		);

		await _dbContext.SaveChangesAsync();
		return $"{myUserId} is now subscribed to ${id}";
    }


	[HttpGet("[action]"), AllowAnonymous]
    public IEnumerable<User> Search(
		[FromQuery(Name = "username")] 
		string username
	){
		return _dbContext.Users
			.Where(u => u.Name.ToLower().Contains(username.ToLower()))
			.OrderByDescending(u => u.Name.ToLower().StartsWith(username.ToLower()));
    }
	
	[HttpPost("[action]"), AllowAnonymous]
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

			StatusCode(201);
			return "OK";
		}

		if(users.Find(u => u.Email == dto.Email) != null) {
			return "Email address already in use";
		}

		return "Username already in use";
	}

	[HttpPost("[action]")]
	[AllowAnonymous]
	public async Task<string?> LogIn(LogInDto? dto)
	{
		return dto != null ? LogInWithDto(dto) : await LogInWithToken();
	}

	private async Task<string?> LogInWithToken() {
		int Id = User.GetUserId();
		if (Id == -1) return null;
		User? user = await _dbContext.Users.FindAsync(Id);
		return user == null ? null : GenerateJWT(user);
	}

	private string? LogInWithDto(LogInDto dto) {
			Console.WriteLine(dto.Email);
		string hashedPassword = dto.Password?.ToSHA256HashedString() ?? string.Empty;
		User? user = _dbContext.Users
			.Where(u => u.Email == dto.Email && u.Password == hashedPassword)
			.FirstOrDefault();
		
		return user == null ? null : GenerateJWT(user);
	}

	private string GenerateJWT(User user)
	{
		var key = _config["Jwt:PrivateKey"] ?? string.Empty;
		var issuer = _config["Jwt:Issuer"] ?? string.Empty;
		return new JwtBuilder(key, issuer)
			.WithClaim(new(nameof(user.Id), user.Id.ToString()))
			.WithClaim(new(nameof(user.Email), user.Email.ToString()))
			.Build();
	}

}
