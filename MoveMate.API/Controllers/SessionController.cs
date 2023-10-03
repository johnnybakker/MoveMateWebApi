using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using MoveMate.Repositories;
using Microsoft.AspNetCore.Authorization;
using MoveMate.Models.Data;
using MoveMate.Models;
using MoveMate.Models.Dto;
using Newtonsoft.Json;

namespace MoveMate.Controllers;


public class SessionController : ApiController
{
	protected readonly SessionRepository _repository;
	
    public SessionController(SessionRepository userRepository)
    {
		_repository = userRepository;
    }

	[HttpGet("{userId}/[action]")]
	public async Task<ApiResult> Refresh(int userId) {
		if(CurrentUser.Id != userId) 
			return ApiResult.Failed();

		string? token = await _repository.Refresh(CurrentSession.Id, TimeSpan.FromHours(1));
		
		if(token == null) 
			return ApiResult.Failed();
		
		return ApiResult.Success(token);
	}

	[HttpPost("{userId}/[action]")]
	public async Task<ApiResult> FirebaseToken (int userId, JsonObject json) {
		
		if(CurrentUser.Id != userId) 
			return ApiResult.Failed();

		string? token = json["token"]?.ToString();
		Console.WriteLine($"Setting firebase token {token}");
		await _repository.SetFirebaseToken(CurrentSession.Id, token);

		return ApiResult.Success();
	}

}