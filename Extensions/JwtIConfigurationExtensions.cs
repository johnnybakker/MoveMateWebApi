		
public static class JwtIConfigurationExtensions
{
    public static string GetJwtPrivateKey(this IConfiguration config) 
		=> config["Jwt:PrivateKey"] ?? string.Empty;

	public static string GetJwtIssuer(this IConfiguration config) 
		=> config["Jwt:Issuer"] ?? string.Empty;

}