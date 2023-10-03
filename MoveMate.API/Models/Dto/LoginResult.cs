namespace MoveMate.Models.Dto;

public class LoginResult {
	public int Id { get; set; } = default;
	public string Username { get; set; } = "";
	public string Email { get; set; } = "";
	public string Token { get; set; } = "";
	public IEnumerable<int> Subscribers { get; set; } = default!;
	public IEnumerable<int> Subscriptions { get; set; } = default!;
}