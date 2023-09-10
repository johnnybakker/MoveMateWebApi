


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(Id)), JsonConverter(typeof(EntityConverter))]
public abstract class Entity {

	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; }

}