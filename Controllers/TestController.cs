

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoveMateWebApi.Controllers;

public class TestController : ApiController {

	[HttpGet, AllowAnonymous]
	public string Get() {



		return "OK";
	}



}