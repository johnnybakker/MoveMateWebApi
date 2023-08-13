using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace MoveMateWebApi.Authentication;

public class JwtBuilder
{
	private readonly String Issuer;
    private readonly JwtHeader Header;
    private readonly IList<Claim> Claims;
    private readonly DateTime StartDate;
    public readonly int LifetimeInSeconds;

    private DateTime ExpireDate => StartDate.AddSeconds(LifetimeInSeconds);

    public JwtBuilder(string privateKey, string issuer)
    {
        var ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        var credentials = new SigningCredentials(ssKey, SecurityAlgorithms.HmacSha256);
        Header = new JwtHeader(credentials);
        Claims = new List<Claim>();
        StartDate = DateTime.UtcNow;
        LifetimeInSeconds = 60 * 60 * 8; // 1 hour
		Issuer = issuer;
    }

    public JwtBuilder WithClaim(Claim claim)
    {
        Claims.Add(claim);
        return this;
    }

    public string Build()
    {
        JwtPayload payload = new(Issuer, Issuer, Claims, StartDate, ExpireDate);
        JwtSecurityToken token = new(Header, payload);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static DateTime GetTokenExpireDateTime(string token)
    {
        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
        }
        catch (Exception)
        {
            return DateTime.MinValue;
        }
    }
}