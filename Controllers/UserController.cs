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
		
		return ApiResult.Success(user);
	}

	[HttpPost("[action]")]
    async public Task<ApiResult> Subscribe(JsonObject body)
    {
		int subscriberId = CurrentSession?.UserId ?? -1;
		int subscriptionId = body["id"]?.GetValue<int>() ?? -1;
		bool result = await _repository.Subscribe(subscriberId, subscriptionId);

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
