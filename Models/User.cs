using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
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

	[Required, JsonIgnore]
	public ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();

	[JsonPropertyName("subscriptions")]
	public ICollection<int> SubscriptionIds => Subscriptions.Select(x => x.ToUserId).ToList();

	[Required, JsonIgnore]	
	public List<Subscription> Subscribers { get; } = new List<Subscription>();

	[JsonPropertyName("subscribers")]
	public ICollection<int> SubscriberIds => Subscribers.Select(x => x.UserId).ToList();

}