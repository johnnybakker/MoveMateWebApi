using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Models.Data;

public class Workout {

	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default!;
	
	[Required]
	public User User { get; set; } = null!;

	[Required]
	public int UserId { get; set; } = default!;

	[Required]
	public DateTime StartDate { get; set; } = default;

	public DateTime? EndDate { get; set; } = default;

	public List<WorkoutData> Data { get; set; } = default!;

	[ForeignKey("TypeId"), Required]
	public EnumEntity<WorkoutType> TypeEntity { get; set; } = default!;

	[Required]
	public int TypeId {get;set;}

	[JsonIgnore, NotMapped]
	public WorkoutType Type { 
		get => (WorkoutType)TypeId; 
		set => TypeId = (int)value; 
	}

	[JsonPropertyName("Type")]
	public string TypeName => Enum.GetName(Type)!;
}