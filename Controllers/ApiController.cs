using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
	public Session? Session => HttpContext?.Items["Session"] as Session;

}
