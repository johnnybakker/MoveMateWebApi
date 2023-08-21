namespace MoveMateWebApi.Models.Dto;

public class LoginResult {
	public string Token { get; set; } = "";
	public string Username { get; set; } = "";
	public string Email { get; set; } = "";
}