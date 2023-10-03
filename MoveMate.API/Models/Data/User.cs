using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoveMate.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class User : Entity {
	
	[MaxLength(100), Required]
	public string Username { get; set; } = default!;
	
	[MaxLength(255), Required]
	public string Email { get; set; } = default!;

	[MaxLength(64), Required, JsonIgnore]
	public string Password { get; set; } = default!;

	[Required, JsonIgnore]
	public virtual ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();
	
	[Required, JsonIgnore]	
	public virtual ICollection<Subscription> Subscribers { get; } = new List<Subscription>();

	[JsonPropertyName("subscriptions")]
	public IEnumerable<int> SubscriptionUserIds => Subscriptions.Select(s => s.ToUserId);

	[JsonPropertyName("subscribers")]
	public IEnumerable<int> SubscriberUserIds => Subscribers.Select(s => s.UserId);
	
	[Required]	
	public virtual List<Session> Sessions { get; } = new List<Session>();

	[Required]	
	public virtual List<Workout> Workouts { get; } = new List<Workout>();
}