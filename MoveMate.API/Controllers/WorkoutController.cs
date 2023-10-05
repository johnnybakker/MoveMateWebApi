

using System.Text.Json.Nodes;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveMate.Models;
using MoveMate.Models.Data;
using MoveMate.API.Repositories;
using MoveMate.API.Services;

namespace MoveMate.Controllers;

public class WorkoutController : ApiController {

	private readonly INotificationService _service;
	private readonly IUserRepository _userRepository;
	private readonly IWorkoutRepository _workoutRepository;

	public WorkoutController(INotificationService service, IWorkoutRepository workoutRepository, IUserRepository userRepository) {
		_service = service;
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
		

		if(tokens.Count() == 0) {
			return ApiResult.Success(workout);
		}

		await _service.BroadcastAsync($"{CurrentSession.User.Username} started {workout.TypeName}", "Started", tokens);


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