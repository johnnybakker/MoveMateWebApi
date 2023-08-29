using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

[PrimaryKey(nameof(Id))]
public class EnumEntity<T> where T: struct, Enum {

	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
	public int Id { get; set; } = default!;

	[Required]
	public string Name { get; set; } = string.Empty;

	[IgnoreDataMember]
	public T Value => Enum.GetValues<T>().First(e => (int)(object)e == Id);


	public static IEnumerable<EnumEntity<T>> Data => Enum.GetValues<T>().Select(e => new EnumEntity<T> {
		Id = Convert.ToInt32(e),
		Name = Enum.GetName(e)!,
	});
}