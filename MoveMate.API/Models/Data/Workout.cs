using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoveMate.Models;

namespace MoveMate.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class Workout : Entity {
	
	[Required, JsonIgnore]
	public virtual User User { get; set; } = null!;
	public int UserId { get; set; }

	[Required]
	public DateTime StartDate { get; set; } = default;

	public DateTime? EndDate { get; set; } = default;

	public virtual List<WorkoutData> Data { get; set; } = default!;

	[Required]
	public virtual EnumEntity<WorkoutType> TypeEntity { get; set; } = default!;
	public int TypeId { get; set; }

	[JsonIgnore, NotMapped]
	public WorkoutType Type { 
		get => (WorkoutType) TypeId; 
		set => TypeId = Convert.ToInt32(value);
	}

	[JsonPropertyName("Type")]
	public string TypeName => Enum.GetName(Type)!;
}