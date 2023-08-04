using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoveMateWebApi.Models;

public class User {
	
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; } = 0;
	
	[MaxLength(100), Required]
	public string Name { get; set; } = string.Empty;
	
	[MaxLength(255), Required]
	public string Email { get; set; } = string.Empty;
}