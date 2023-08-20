
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoveMateWebApi.Models;

public class Subscription {
	
	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default;

	[Required]
	public User User { get; set; } = null!;

	[Required]
	public int UserId { get; set; } = default;

	[Required]
	public User ToUser { get; set; } = null!;
	[Required]
	public int ToUserId { get; set; } = default;


	[Required]
	public bool IsFavorite { get; set; } = false;
} 