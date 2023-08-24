using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MoveMateWebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MoveMateWebApi.Models.Data;

namespace MoveMateWebApi.Services;

public class JwtTokenFactory : ITokenFactory
{
	private readonly string _issuer;
    private readonly JwtHeader _header;
	
    public JwtTokenFactory(IConfiguration config)
    {
		var key = config.GetJwtPrivateKey();
        var ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(ssKey, SecurityAlgorithms.HmacSha256);
		_header = new JwtHeader(credentials);
		_issuer = config.GetJwtIssuer();
    }

	public Task<string> Create(Session session)
	{
		var claims = new List<Claim>() {
			new(nameof(Session.Id), session.Id.ToString()),
			new(nameof(Session.UserId), session.UserId.ToString())
		};

        var payload = new JwtPayload(_issuer, _issuer, claims, null, session.ExpirationDate);
        var token = new JwtSecurityToken(_header, payload);
		var handler = new JwtSecurityTokenHandler();

        return Task.FromResult(handler.WriteToken(token));
	}
}