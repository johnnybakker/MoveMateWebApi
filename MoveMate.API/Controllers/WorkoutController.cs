

using System.Text.Json.Nodes;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveMate.Models;
using MoveMate.Models.Data;
using MoveMate.Repositories;
using MoveMate.Services;

namespace MoveMate.Controllers;

public class WorkoutController : ApiController {

	private readonly FireBaseService _service;
	private readonly SessionRepository _sessionRepository;
	private readonly UserRepository _userRepository;
	private readonly WorkoutRepository _workoutRepository;

	public WorkoutController(FireBaseService service, SessionRepository sessionRepository, WorkoutRepository workoutRepository, UserRepository userRepository) {
		_service = service;
		_sessionRepository = sessionRepository;
		_workoutRepository = workoutRepository;
		_userRepository = userRepository;
	}


	[HttpPost]
	public async Task<ApiResult> Create(JsonObject body) {

		string? type = body["type"]?.ToString();

		WorkoutType? workoutType = Enum.GetValues<WorkoutType>()
			.FirstOrDefault(e => Enum.GetName(e)?.ToLower() == type?.ToLower());

		if(workoutType == null) return ApiResult.Failed();

		Workout? workout = await _workoutRepository.Create(CurrentUser.Id, (WorkoutType)workoutType);

		if(workout == null) return ApiResult.Failed();

		IEnumerable<string> tokens = await _userRepository.GetSubscriberFirebaseTokens(CurrentSession.UserId);
		
		foreach(string token in tokens) {
			Console.WriteLine(token);
		}


		if(tokens.Count() == 0) {
			Console.WriteLine("No subscibers");
			return ApiResult.Success(workout);
		}

		var message = new MulticastMessage {
			Notification = new Notification 
			{
				Title = $"{CurrentSession.User.Username} started {workout.TypeName}",
				Body = "Started"
			},
			Tokens = tokens.ToList()
		};

		var result = await _service.messaging.SendMulticastAsync(message);
		if(result.SuccessCount == tokens.Count()) {
		
				Console.WriteLine("Multicast is sent");
		} else {
			foreach(var response in result.Responses)
				Console.WriteLine($"{response.MessageId} {(response.IsSuccess ? "Success" : response.Exception.Message)}");
		}


		return ApiResult.Success(workout);
	}

	[HttpGet("{id}")]
	public async Task<ApiResult> Get(int id) {
		Workout? workout = await _workoutRepository.Get(id);
		if(workout == null) return ApiResult.Failed();
		return ApiResult.Success(workout);
	}

	[HttpGet]
	public async Task<ApiResult> Get() {
		IEnumerable<Workout> workouts = await _workoutRepository.GetByUserId(CurrentSession.UserId);
		return ApiResult.Success(workouts);
	}
}