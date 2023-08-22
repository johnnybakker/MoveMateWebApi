namespace MoveMateWebApi.Models.Dto;

public class LoginResult {
	public int Id { get; set; } = default;
	public string Username { get; set; } = "";
	public string Email { get; set; } = "";
	public string Token { get; set; } = "";
}