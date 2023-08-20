using Microsoft.AspNetCore.Mvc;
using MoveMateWebApi.Models;
using System.Text.Json.Nodes;
using MoveMateWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MoveMateWebApi.Controllers;


public class UserController : ApiController
{
	protected readonly UserRepository _repository;
	
    public UserController(UserRepository userRepository)
    {
		_repository = userRepository;
    }

	[HttpGet("[action]"), AllowAnonymous]
    public string Test()
    {
		return  "Hello";
    }


    [HttpGet]
    public IEnumerable<User> Get()
    {
		return _repository.GetAll();
    }

	[HttpPost("[action]")]
    async public Task Subscribe(JsonObject body)
    {
		int subscriberId = Session?.UserId ?? -1;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		await _repository.Subscribe(subscriberId, subscriptionId);
    }


	[HttpGet("[action]"), AllowAnonymous]
    public IEnumerable<User> Search([FromQuery(Name = "username")] string username){
		return _repository.Search(username);
    }
	
	[HttpPost("[action]"), AllowAnonymous]
	public async Task<string?> SignUp(SignUpDto dto) {
		return await _repository.SignUp(dto);
	}

	[HttpPost("[action]"), AllowAnonymous]
	public async Task<string?> Login(LogInDto? dto)
	{
			Console.WriteLine("Login");

		if(dto != null) {
			Console.WriteLine("Login using dto");

			return await _repository
				.LoginUsingEmailAndPassword(dto.Email ?? "", dto.Password ?? "");
		}

		if(Session != null) {
			return await _repository
				.LoginUsingSession(Session);
		}

		return null;
	}
}
