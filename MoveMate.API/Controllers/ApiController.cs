using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveMate.Models.Data;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
	public Session CurrentSession => (Session)HttpContext.Items["Session"]!;
	public User CurrentUser => (User)HttpContext.Items["SessionUser"]!;
}
