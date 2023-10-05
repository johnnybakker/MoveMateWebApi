using HttpContextMoq;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using MoveMate.API.Database;
using MoveMate.API.Repositories;
using MoveMate.API.Services;
using MoveMate.Models.Data;

namespace MoveMate.UnitTests.Helpers
{
    public abstract class ApiControllerTest : IDisposable
    {
        public List<User> Users = new List<User>();
        public List<Session> Sessions = new List<Session>();
        public List<Subscription> Subscriptions = new List<Subscription>();
        public List<Workout> Workouts = new List<Workout>();
        public List<WorkoutData> WorkoutData = new List<WorkoutData>();
		public List<EnumEntity<WorkoutType>> WorkoutTypes = new List<EnumEntity<WorkoutType>>();


        private Mock<MoveMateDbContext> DatabaseMock;
        private MoveMateDbContext Database => DatabaseMock.Object;

        public HttpContextMock HttpContext;

        protected IUserRepository UserRepository { get; set; }
        protected IWorkoutRepository WorkoutRepository { get; set; }
        protected ISessionRepository SessionRepository { get; set; }
        protected ITokenFactory TokenFactory { get; set; }
        protected IConfiguration Configuration { get; set; }
		
		protected User? CurrentUser { 
			get => HttpContext.Items["SessionUser"] as User; 
			set => HttpContext.Items["SessionUser"] = value; 
		}

		protected Session? CurrentSession { 
			get => HttpContext.Items["Session"] as Session; 
			set => HttpContext.Items["Session"] = value; 
		}

        public ApiControllerTest() {
			
            HttpContext = new HttpContextMock()
            {
                Items = new Dictionary<object, object?> {
					{ "SessionUser", null },
					{ "Session", null }, 
            	}
            };

			SetupTestData();

            DatabaseMock = new Mock<MoveMateDbContext>(new TestConfiguration());
            DatabaseMock.Setup(x => x.Users).ReturnsDbSet(Users);
            DatabaseMock.Setup(x => x.Sessions).ReturnsDbSet(Sessions);
            DatabaseMock.Setup(x => x.Subscriptions).ReturnsDbSet(Subscriptions);
            DatabaseMock.Setup(x => x.Workouts).ReturnsDbSet(Workouts);
            DatabaseMock.Setup(x => x.WorkoutData).ReturnsDbSet(WorkoutData);
            DatabaseMock.Setup(x => x.WorkoutTypes).ReturnsDbSet(WorkoutTypes);


            Configuration = new TestConfiguration();
            TokenFactory = new JwtTokenFactory(Configuration);
            SessionRepository = new SessionRepository(Database, TokenFactory);
            UserRepository = new UserRepository(Database, TokenFactory, SessionRepository);
            WorkoutRepository = new WorkoutRepository(Database);
		}

		protected abstract void SetupTestData();
		protected abstract void ClearTestData();

		public void Dispose()
		{
			ClearTestData();
		}
	}
}
