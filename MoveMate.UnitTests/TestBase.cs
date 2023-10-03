using HttpContextMoq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using MoveMate.API.Database;
using MoveMate.API.Repositories;
using MoveMate.API.Services;
using MoveMate.Controllers;
using MoveMate.Models.Data;

namespace MoveMate.UnitTests
{
    public abstract class TestBase
    {
        public static List<User> Users = new List<User>()
        {
            new User { Id = 1, Username = "Johnny", Email = "johnny@test.nl", Password = "Test123!".ToSHA256HashedString() },
            new User { Id = 2, Username = "Johnny2", Email = "johnny2@test.nl", Password = "Test123!".ToSHA256HashedString() }
        };

        public static List<Session> Sessions = new List<Session>();
        public static List<Subscription> Subscriptions = new List<Subscription>();
        public static List<Workout> Workouts = new List<Workout>();
        public static List<WorkoutData> WorkoutData = new List<WorkoutData>();


        private Mock<MoveMateDbContext> DatabaseMock;
        public MoveMateDbContext Database => DatabaseMock.Object;

        public HttpContextMock HttpContext;

        protected IUserRepository UserRepository { get; set; }
        protected IWorkoutRepository WorkoutRepository { get; set; }
        protected ISessionRepository SessionRepository { get; set; }
        protected ITokenFactory TokenFactory { get; set; }
        protected IConfiguration Configuration { get; set; }

        public TestBase() {
            HttpContext = new HttpContextMock()
            {
                Items = new Dictionary<object, object> {
                { "SessionUser", Users[0] },
                { "Session", new Session()
                    {
                        Id = 1,
                        ExpirationDate = DateTime.UtcNow.AddHours(1),
                        FirebaseToken = null,
                        Token = "",
                        User = Users[0],
                        UserId = Users[0].Id
                    }
                }
            }
            };

            DatabaseMock = new Mock<MoveMateDbContext>(new TestConfiguration());
            DatabaseMock.Setup(x => x.Users).ReturnsDbSet(Users);
            DatabaseMock.Setup(x => x.Sessions).ReturnsDbSet(Sessions);
            DatabaseMock.Setup(x => x.Subscriptions).ReturnsDbSet(Subscriptions);
            DatabaseMock.Setup(x => x.Workouts).ReturnsDbSet(Workouts);
            DatabaseMock.Setup(x => x.WorkoutData).ReturnsDbSet(WorkoutData);

            Configuration = new TestConfiguration();
            TokenFactory = new JwtTokenFactory(Configuration);
            SessionRepository = new SessionRepository(Database, TokenFactory);
            UserRepository = new UserRepository(Database, TokenFactory, SessionRepository);
            WorkoutRepository = new WorkoutRepository(Database);
        }
    }
}
