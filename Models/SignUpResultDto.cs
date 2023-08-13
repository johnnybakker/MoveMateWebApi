public class SignUpResultDto {
	public bool IsValidEmail { get; set; } = true;
	public bool IsStrongPassword { get; set; } = true;
	public bool UserNameAlreadyExists {get;set;} = false;
	public bool EmailAlreadyExists {get;set;} = false;
	// public bool PasswordContainsUpperCaseCharacters { get; set; } = false;
	// public bool PasswordContainsThreeLowerCaseCharacters { get; set; } = false;
	// public bool PasswordContainsSpecialCharacters { get; set; } = false;
	// public bool PasswordContainsEightCharacters { get; set; } = false;
}