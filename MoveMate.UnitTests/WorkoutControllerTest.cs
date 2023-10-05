using System.Text.Json.Nodes;
using Moq;
using MoveMate.API.Services;
using MoveMate.Controllers;
using MoveMate.Models;
using MoveMate.Models.Api;
using MoveMate.Models.Data;

namespace MoveMate.UnitTests;

public class WorkoutControllerTest : ApiControllerTest
{


	WorkoutController Controller;
	INotificationService NotificationService;

    public WorkoutControllerTest() : base()
    {
		var notificationServiceMock = new Mock<INotificationService>();
		
		notificationServiceMock.Setup(s => s.BroadcastAsync(
			It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()));

		NotificationService = notificationServiceMock.Object;
	
        Controller = new WorkoutController(NotificationService, SessionRepository, WorkoutRepository, UserRepository);
        Controller.ControllerContext.HttpContext = HttpContext;
    }

	protected override void SetupTestData()
	{
		TestData data = new TestData();

		Users.Add(data.TestUser1);
		Users.Add(data.TestUser2);
		Sessions.Add(data.TestSession);
		Workouts.Add(data.TestWorkout1);
		Workouts.Add(data.TestWorkout2);

		CurrentUser = Users[0];
		CurrentSession = Sessions[0];
	}

	protected override void ClearTestData()
	{
		Users.Clear();
		Sessions.Clear();
		Workouts.Clear();

		CurrentSession = null;
		CurrentUser = null;
	}

	[Fact]
	public async void TestGetWorkouts() 
	{
		ApiResult result = await Controller.Get();
		Assert.Equal(ApiResultType.Success, result.Type);
		
		List<Workout>? workouts;
		Assert.True(result.Unpack(out workouts));
		Assert.NotNull(workouts);
		Assert.Single(workouts);
	}

	[Fact]
	public async void TestGetOneWorkout() 
	{
		ApiResult result1 = await Controller.Get(1);
		Assert.Equal(ApiResultType.Success, result1.Type);
		
		Workout? workout1;
		Assert.True(result1.Unpack(out workout1));
		Assert.NotNull(workout1);

		ApiResult result2 = await Controller.Get(2);
		Assert.Equal(ApiResultType.Success, result2.Type);
		
		Workout? workout2;
		Assert.True(result2.Unpack(out workout2));
		Assert.NotNull(workout2);
	}

	[Fact]
	public async void TestCreateWorkout() 
	{
		JsonObject body = JsonNode.Parse("{ \"type\": \"RUNNING\" }")?.AsObject()!;
		ApiResult result = await Controller.Create(body);
		Assert.Equal(ApiResultType.Success, result.Type);
		
		Workout? workout;
		Assert.True(result.Unpack(out workout));
		Assert.NotNull(workout);
		Assert.Equal(Controller.CurrentUser.Id, workout.UserId);
		Assert.Equal(WorkoutType.RUNNING, workout.Type);
	}
}
