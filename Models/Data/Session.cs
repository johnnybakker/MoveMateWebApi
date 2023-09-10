using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class Session : Entity {

	[Required] 
	public virtual User User { get; set; } = null!;
	public int UserId { get; set; }

	[Required] 
	public DateTime ExpirationDate { get; set; } = default;

	public string? Token { get; set; } = null;
	public string? FirebaseToken {get; set; } = null; 
}