using MoveMate.Models.Data;

namespace MoveMate.UnitTests.Data;



public class TestData {


	public User TestUser1 = new User { 
		Id = 1, 
		Username = "Johnny", 
		Email = "johnny@test.nl", 
		Password = "Test123!".ToSHA256HashedString() 
	};

	public User TestUser2 = new User { 
		Id = 2, 
		Username = "Johnn2", 
		Email = "johnn2@test.nl", 
		Password = "Test123!".ToSHA256HashedString() 
	};

	public Session TestSession = new Session { 
		Id = 1, 
		ExpirationDate = DateTime.Now.AddHours(1), 
		FirebaseToken = null, 
		Token = string.Empty 
	};

	public static EnumEntity<WorkoutType> RunningWorkoutType => EnumEntity<WorkoutType>.GetEntity(WorkoutType.RUNNING);
	public static EnumEntity<WorkoutType> WalkingWorkoutType => EnumEntity<WorkoutType>.GetEntity(WorkoutType.WALKING);

	public Workout TestWorkout1 = new Workout{ 
		Id = 1,
		StartDate = DateTime.Now,
		EndDate = DateTime.Now,
		Data = new List<WorkoutData>(),
		Type = RunningWorkoutType.Value,
		TypeEntity = RunningWorkoutType,
		TypeId = RunningWorkoutType.Id
	};

	public Workout TestWorkout2 = new Workout{ 
		Id = 2,
		StartDate = DateTime.Now,
		EndDate = DateTime.Now,
		Data = new List<WorkoutData>(),
		Type = WalkingWorkoutType.Value,
		TypeEntity = WalkingWorkoutType,
		TypeId = WalkingWorkoutType.Id
	};

	public List<EnumEntity<WorkoutType>> WorkoutTypes = EnumEntity<WorkoutType>.Entities.ToList();

	public TestData() 
	{
		TestSession.User = TestUser1;
		TestSession.UserId = TestUser1.Id;
		
		TestWorkout1.User = TestUser1;
		TestWorkout1.UserId = TestUser1.Id;

		TestWorkout2.User = TestUser2;
		TestWorkout2.UserId = TestUser2.Id;

		TestUser1.Sessions.Add(TestSession);
		TestUser1.Workouts.Add(TestWorkout1);
		TestUser2.Workouts.Add(TestWorkout2);
	}



}