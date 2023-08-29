using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveMateWebApi.Models.Data;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
	public Session CurrentSession => (Session)HttpContext.Items["Session"]!;
	public Session CurrentUser => (Session)HttpContext.Items["SessionUser"]!;
}
