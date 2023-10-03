using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoveMate.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class Subscription : Entity {
	
	[Required]
	public virtual User User { get; set; } = null!;
	public int UserId { get; set; }

	[Required]
	public virtual User ToUser { get; set; } = null!;
	public int ToUserId { get; set; }

	[Required]
	public bool IsFavorite { get; set; } = false;
} 