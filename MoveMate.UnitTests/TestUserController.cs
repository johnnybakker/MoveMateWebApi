using MoveMate.Controllers;
using MoveMate.Models.Data;
using MoveMate.Models.Api;
using MoveMate.Models.Dto;
using MoveMate.Models;
using System.Text.Json.Nodes;

namespace MoveMate.UnitTests;

public class TestUserController : TestBase
{
    UserController Controller;

    public TestUserController() : base()
    {
        Controller = new UserController(UserRepository);
        Controller.ControllerContext.HttpContext = HttpContext;
    }

    [Fact]
    public async Task TestGetAllUsers()
    {
        ApiResult result = await Controller.Get();
        List<User>? users;
        Assert.NotNull(result);
        Assert.Equal(ApiResultType.Success, result.Type);
        Assert.True(result.Unpack(out users));
        Assert.NotNull(users);
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async Task TestLoginUsingEmailAndPassword()
    {
        ApiResult result = await Controller.Login(new LoginRequest { Email = "johnny@test.nl", Password = "Test123!" });

        LoginResult? loginResult;
        Assert.Equal(ApiResultType.Success, result.Type);
        Assert.True(result.Unpack(out loginResult));
        Assert.NotNull(loginResult);
        Assert.Equal(1, loginResult.Id);
    }

    [Fact]
    public async Task TestLoginUsingSession()
    {
        ApiResult result = await Controller.Login(1);

        LoginResult? loginResult;
        Assert.Equal(ApiResultType.Success, result.Type);
        Assert.True(result.Unpack(out loginResult));
        Assert.NotNull(loginResult);
        Assert.Equal(Controller.CurrentUser.Id, loginResult.Id);
    }

    [Fact]
    public async Task TestSubscribeAndUnsubscribe()
    {    
        ApiResult result = await Controller.Subscribe(1, JsonNode.Parse("{ \"id\": 2 }")?.AsObject()!);

        int subscriptionId;
        Assert.Equal(ApiResultType.Success, result.Type);
        Assert.True(result.Unpack(out subscriptionId));
        Assert.Equal(2, subscriptionId);

        result = await Controller.UnSubscribe(1, JsonNode.Parse("{ \"id\": 2 }")?.AsObject()!);
        Assert.Equal(ApiResultType.Success, result.Type);
        Assert.True(result.Unpack(out subscriptionId));
        Assert.Equal(2, subscriptionId);
    }
}