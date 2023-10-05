using System.Text.Json.Nodes;
using MoveMate.Controllers;
using MoveMate.Models;
using MoveMate.Models.Api;

namespace MoveMate.UnitTests;

public class SessionControllerTest : ApiControllerTest
{


	SessionController Controller;
    public SessionControllerTest() : base()
    {
        Controller = new SessionController(SessionRepository);
        Controller.ControllerContext.HttpContext = HttpContext;
    }

	protected override void SetupTestData()
	{
		TestData data = new TestData();

		Users.Add(data.TestUser1);
		Sessions.Add(data.TestSession);

		CurrentUser = Users[0];
		CurrentSession = Sessions[0];
	}

	protected override void ClearTestData()
	{
		Users.Clear();
		Sessions.Clear();

		CurrentSession = null;
		CurrentUser = null;
	}

	[Fact]
	public async void TestSetFirebaseTokenSuccess() 
	{
		JsonObject body = JsonNode.Parse("{ \"token\": \"1234567890\" }")?.AsObject()!;
		ApiResult result = await Controller.FirebaseToken(Controller.CurrentUser.Id, body);
		Assert.Equal(ApiResultType.Success, result.Type);
	}

	[Fact]
	public async void TestSetFirebaseTokenFailed() 
	{
		JsonObject body = JsonNode.Parse("{ \"token\": \"1234567890\" }")?.AsObject()!;
		ApiResult result = await Controller.FirebaseToken(0, body);
		Assert.Equal(ApiResultType.Failed, result.Type);
	}


	[Fact]
	public async void TestRefreshSession() 
	{
		ApiResult result = await Controller.Refresh(Controller.CurrentUser.Id);
		Assert.Equal(ApiResultType.Success, result.Type);
		
		string? token;
		Assert.True(result.Unpack(out token));
		Assert.NotNull(token);
	}


}
