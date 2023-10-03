using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoveMate.Models.Data;

[JsonConverter(typeof(EntityConverter))]
public class EnumEntity<T> : Entity where T: struct, Enum {

	[Required]
	public string Name { get; set; } = string.Empty;

	[IgnoreDataMember]
	public T Value => Enum.GetValues<T>().First(e => (int)(object)e == Id);

	public static EnumEntity<T> GetEntity(T value) {
		return Entities.First(e => e.Value.CompareTo(value) == 0);
	}

	public static readonly IEnumerable<EnumEntity<T>> Entities = Enum.GetValues<T>().Select(e => new EnumEntity<T> {
		Id = Convert.ToInt32(e),
		Name = Enum.GetName(e)!,
	});
}