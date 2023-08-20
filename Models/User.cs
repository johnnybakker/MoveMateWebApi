using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoveMateWebApi.Models;

public class User {
	
	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default;
	
	[MaxLength(100), Required]
	public string Name { get; set; } = default!;
	
	[MaxLength(255), Required]
	public string Email { get; set; } = default!;

	[MaxLength(64), Required, JsonIgnore]
	public string Password { get; set; } = default!;

	[Required]
	public ICollection<Subscription> Subscriptions { get; } = 
		new List<Subscription>();

	[Required]	
	public List<Subscription> Subscribers { get; } = 
		new List<Subscription>();
}