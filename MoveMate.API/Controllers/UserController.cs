using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using MoveMate.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using MoveMate.Models.Data;
using MoveMate.Models;
using MoveMate.Models.Dto;
namespace MoveMate.Controllers;


public class UserController : ApiController
{
	protected readonly IUserRepository _repository;
	
    public UserController(IUserRepository userRepository)
    {
		_repository = userRepository;
    }

    [HttpGet]
    public async Task<ApiResult> Get() {
		return ApiResult.Success(await _repository.GetAll());
	}

	[HttpGet("{id}")]
    public async Task<ApiResult> Login(int id) {
		if(CurrentUser.Id != id)
			throw new UnauthorizedAccessException();

		User? user = await _repository.Get(id);
		
		if(user == null) return ApiResult.Failed();
		
		var result = new LoginResult() {
			Email = user.Email,
			Id = user.Id,
			Token = CurrentSession.Token ?? "",
			Username = user.Username,
			Subscribers = await _repository.GetSubscribers(user.Id),
			Subscriptions = await _repository.GetSubscriptions(user.Id)
		};
		
		return ApiResult.Success(result);
	}

	[HttpGet("{id}/[action]")]
    public async Task<ApiResult> Subscribers(int id) {
		if(CurrentUser.Id != id)
			throw new UnauthorizedAccessException();

		var subscribers = await _repository.GetSubscribers(id);
		
		return ApiResult.Success(subscribers);
	}

	[HttpGet("{id}/[action]")]
    public async Task<ApiResult> Subscriptions(int id) {
		if(CurrentUser.Id != id)
			throw new UnauthorizedAccessException();

		var subscriptions = await _repository.GetSubscriptions(id);
		
		return ApiResult.Success(subscriptions);
	}

	[HttpPost("{id}/[action]")]
    async public Task<ApiResult> Subscribe(int id, JsonObject body)
    {
		if(CurrentUser.Id != id)
			throw new UnauthorizedAccessException();

		int subscriberId = CurrentUser.Id;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		bool result = await _repository.Subscribe(subscriberId, subscriptionId);

		return result ? ApiResult.Success(subscriptionId) : ApiResult.Failed();
    }

	[HttpPost("{id}/[action]")]
    async public Task<ApiResult> UnSubscribe(int id, JsonObject body)
    {
		if(CurrentUser.Id != id)
			throw new UnauthorizedAccessException();

		int subscriberId = CurrentUser.Id;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		bool result = await _repository.UnSubscribe(subscriberId, subscriptionId);

		return result ? ApiResult.Success(subscriptionId) : ApiResult.Failed();
    }


	[HttpGet("[action]"), AllowAnonymous]
    public async Task<ApiResult> Search([FromQuery(Name = "username")] string username){
		return ApiResult.Success(await _repository.Search(username));
    }
	
	[HttpPost("[action]"), AllowAnonymous]
	public async Task<ApiResult> SignUp(SignUpRequest request) {
		SignUpResult result = await _repository.SignUp(request);
		return result.IsValid ? ApiResult.Success() : ApiResult.Failed(result);
	}

	[HttpPost("[action]"), AllowAnonymous]
	public async Task<ApiResult> Login(LoginRequest request)
	{
		LoginResult? result = await _repository.Login(request);
		return result == null ? ApiResult.Failed() : ApiResult.Success(result);
	}
}
