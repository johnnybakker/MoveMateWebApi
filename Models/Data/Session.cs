using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MoveMateWebApi.Models;

namespace MoveMateWebApi.Models.Data;

public class Session {

	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default!;
	
	[Required]
	public User User { get; set; } = null!;

	[Required]
	public int UserId { get; set; } = default!;

	[Required]
	public bool Expired { get; set; } = false;
}