using System.Text.RegularExpressions;

public static class StringValidationExtensions {
	public static bool IsValidEmailAddress(this string str) =>
		new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(str);

	public static bool ContainsLowerCaseCharacters(this string str, int amount = 1) =>
		new Regex($"^(?=(.*[a-z]){{{amount},}}).*$").IsMatch(str);

	public static bool ContainsUpperCaseCharacters(this string str, int amount = 1) =>
		new Regex($"^(?=(.*[A-Z]){{{amount},}}).*$").IsMatch(str);

	public static bool ContainsNumberCharacters(this string str, int amount = 1) =>
		new Regex($"^(?=(.*[0-9]){{{amount},}}).*$").IsMatch(str);

	public static bool ContainsSpecialCharacters(this string str, int amount = 1) =>
		new Regex($"^(?=(.*[!@#\\$%^&*()\\-__+.]){{{amount},}}).*$").IsMatch(str);

	
	public static bool IsStrongPassword(this string str) =>
		str.Length >= 8 &&
		str.ContainsLowerCaseCharacters(3) &&
		str.ContainsSpecialCharacters() &&
		str.ContainsUpperCaseCharacters() &&
		str.ContainsNumberCharacters();
}