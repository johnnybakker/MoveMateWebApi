namespace MoveMateWebApi.Models.Dto;

public class SignUpResult {
	public bool IsValidEmail { get; set; } = true;
	public bool IsStrongPassword { get; set; } = true;
	public bool UserNameAlreadyExists {get;set;} = false;
	public bool EmailAlreadyExists {get;set;} = false;

	public bool IsValid => IsValidEmail && IsStrongPassword && !UserNameAlreadyExists && !EmailAlreadyExists;
}