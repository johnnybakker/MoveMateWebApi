using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using MoveMateWebApi.Models;
namespace MoveMateWebApi.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class WorkoutData : Entity {

	[Required]
	public virtual Workout Workout { get; set; } = null!;
	public int WorkoutId { get; set; }

	[Required]
	public long Time { get; set; } = default;

	[Required]
	public JsonObject Data { get; set; } = new();
	
}