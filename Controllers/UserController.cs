using Microsoft.AspNetCore.Mvc;
using MoveMateWebApi.Database;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	protected readonly MoveMateDbContext _dbContext;
	
    public UserController(MoveMateDbContext dbContext)
    {
		_dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
		return _dbContext.Users.ToList();
    }
}
