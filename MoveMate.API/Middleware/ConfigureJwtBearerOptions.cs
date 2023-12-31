using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MoveMate.Models.Data;
using MoveMate.API.Repositories;

namespace MoveMate.Middleware;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions> {

	private readonly IConfiguration _config;
	private readonly IServiceProvider _provider;

	public ConfigureJwtBearerOptions(IConfiguration config, IServiceProvider provider) 
	{
		//_repository = repository;
		_config = config;
		_provider = provider;
	}

	public void Configure(string? name, JwtBearerOptions options)
	{
		Configure(options);
	}

	public void Configure(JwtBearerOptions options)
	{

		var key = _config.GetJwtPrivateKey();
        var ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

		options.TokenValidationParameters = new() {
			ValidateIssuerSigningKey = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			IssuerSigningKey = ssKey,
			ValidIssuer = _config.GetJwtIssuer(),
			ValidAudience = _config.GetJwtIssuer(),
		};

		options.Events = new() {
			OnTokenValidated = OnTokenValidated
		};
	}

	private async Task OnTokenValidated(TokenValidatedContext context) {	
		if(await AttachSession(context) == false)
			context.Fail("Failed to attach session");
	}

	private async Task<bool> AttachSession(TokenValidatedContext context) {		
		if(context.Principal == null) return false;
		
		string? token =  context.HttpContext.Request.Headers[HeaderNames.Authorization];
		if(token == null) return false;
		token = token.Replace("Bearer", "").Trim();

		var sessionId = GetSessionId(context.Principal) ?? -1;
		if(sessionId == -1) return false;

		var userId = GetUserId(context.Principal) ?? -1;
		if(userId == -1) return false;

			
		using (var scope = _provider.CreateScope()) {
			
			ISessionRepository sessionRepository = scope.ServiceProvider.GetRequiredService<ISessionRepository>();		
			Session? session  = await sessionRepository.Get(sessionId);
		
			if(session == null) return false;
			if(session.UserId != userId) return false;
			if(session.Token != token) return false;

			IUserRepository userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
			User? user  = await userRepository.Get(session.UserId);
			if(user == null) return false;

			session.User = user;
			context.HttpContext.Items["Session"] = session;
			context.HttpContext.Items["SessionUser"] = user;
		}
		
		return true;
	}

	private static int? GetSessionId(ClaimsPrincipal token) 
		=> GetIntFromToken(token, nameof(Session.Id));

	private static int? GetUserId(ClaimsPrincipal token) 
		=> GetIntFromToken(token, nameof(Session.UserId));

	private static int? GetIntFromToken(ClaimsPrincipal token, string key) 
	{
		var claim = GetFromTokenClaim(token, key);
		return int.TryParse(claim?.Value, out int id) ? id : null; 
	}

	private static Claim? GetFromTokenClaim(ClaimsPrincipal token, string type)
		=> token.Claims.FirstOrDefault(c => c.Type == type);
}