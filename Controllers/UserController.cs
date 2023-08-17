using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models;
using MoveMateWebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
		return _dbContext.Users.ToList();
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
