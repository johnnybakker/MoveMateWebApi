

using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveMateWebApi.Repositories;
using MoveMateWebApi.Services;

namespace MoveMateWebApi.Controllers;

public class TestController : ApiController {

	private readonly FireBaseService _service;
	private readonly SessionRepository _sessionRepository;

	public TestController(FireBaseService service, SessionRepository sessionRepository) {
		_service = service;
		_sessionRepository = sessionRepository;
	}

	[HttpGet, AllowAnonymous]
	public async Task<string> Get() {

		List<string> tokens = await _sessionRepository
			.GetAllWithFirebaseToken()
			.ContinueWith(e => e.Result.Select(e => e.FirebaseToken!).ToList());

		if(tokens.Count > 0) {

			var message = new MulticastMessage {
				Notification = new Notification 
				{
					Title = "To everyone",
					Body = "Hello world!"
				},
				Tokens = tokens
			};

			var result = await _service.messaging.SendMulticastAsync(message);

			if (result.SuccessCount > 0)
			{
				// Message was sent successfully
				Console.WriteLine("Message sent successfully!");
			}
			else
			{
				// There was an error sending the message
				Console.WriteLine("Error sending the message.");
			}
		}

		return _service.GetName();
	}



}