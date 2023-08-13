using MoveMateWebApi.Models;
using System.Security.Claims;


public static class ClaimsPrincipalExtensions
{


    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.GetClaim(nameof(User.Name))?.Value;

    public static int GetUserId(this ClaimsPrincipal principal) 
	{
		string? strId = principal.GetClaim(nameof(User.Id))?.Value ?? "-1";
		
		int id;
		return int.TryParse(strId, out id) ? id : -1;
	}

	public static Claim? GetClaim(this ClaimsPrincipal principal, string type)
		=> principal.Claims.FirstOrDefault(c => c.Type == type);
}