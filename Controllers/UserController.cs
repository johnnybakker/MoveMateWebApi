using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using MoveMateWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using MoveMateWebApi.Models.Data;
using MoveMateWebApi.Models;
using MoveMateWebApi.Models.Dto;
using Newtonsoft.Json;

namespace MoveMateWebApi.Controllers;


public class UserController : ApiController
{
	protected readonly UserRepository _repository;
	
    public UserController(UserRepository userRepository)
    {
		_repository = userRepository;
    }

    // [HttpGet]
    // public ApiResult Get() {
	// 	return ApiResult.Success(_repository.GetAll());
	// }

	[HttpGet("{id}")]
    public async Task<ApiResult> Get(int id) {
		if(CurrentSession.UserId != id)
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
		if(CurrentSession.UserId != id)
			throw new UnauthorizedAccessException();

		var subscribers = await _repository.GetSubscribers(id);
		
		return ApiResult.Success(subscribers);
	}

	[HttpGet("{id}/[action]")]
    public async Task<ApiResult> Subscriptions(int id) {
		if(CurrentSession.UserId != id)
			throw new UnauthorizedAccessException();

		var subscriptions = await _repository.GetSubscriptions(id);
		
		return ApiResult.Success(subscriptions);
	}

	[HttpPost("{id}/[action]")]
    async public Task<ApiResult> Subscribe(int id, JsonObject body)
    {
		if(CurrentSession.UserId != id)
			throw new UnauthorizedAccessException();
		int subscriberId = CurrentSession?.UserId ?? -1;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		bool result = await _repository.Subscribe(subscriberId, subscriptionId);

		return result ? ApiResult.Success(subscriptionId) : ApiResult.Failed();
    }

	[HttpPost("{id}/[action]")]
    async public Task<ApiResult> UnSubscribe(int id, JsonObject body)
    {
		if(CurrentSession.UserId != id)
			throw new UnauthorizedAccessException();

		int subscriberId = CurrentSession?.UserId ?? -1;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		bool result = await _repository.UnSubscribe(subscriberId, subscriptionId);

		return result ? ApiResult.Success(subscriptionId) : ApiResult.Failed();
    }


	[HttpGet("[action]"), AllowAnonymous]
    public ApiResult Search([FromQuery(Name = "username")] string username){
		return ApiResult.Success(_repository.Search(username));
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
