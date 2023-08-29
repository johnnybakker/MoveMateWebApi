using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MoveMateWebApi.Models;
using Newtonsoft.Json.Linq;

namespace MoveMateWebApi.Models.Data;

public class WorkoutData {

	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default!;
	
	[Required]
	public Workout Workout { get; set; } = null!;

	[Required]
	public int WorkoutId { get; set; } = default!;

	[Required]
	public long Time { get; set; } = default;

	[Required]
	public JObject Data { get; set; } = new JObject();
}