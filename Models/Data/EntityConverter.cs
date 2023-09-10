

using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EntityConverter : JsonConverter<Entity>
{
	public override bool CanConvert(Type typeToConvert)
	{
		return true;
	}

	public override Entity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return JsonSerializer.Deserialize<Entity>(ref reader, options);
	}

	public Type EntityType => typeof(Entity);
	public PropertyInfo EntityProperty => EntityType.GetProperty(nameof(Entity.Id))!;

	public override void Write(Utf8JsonWriter writer, Entity value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		var type = value.GetType();
		var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		Entity? entity;
		IEnumerable<Entity>? enumerable;

		foreach (PropertyInfo p in properties) 
		{		
			var nameAttr = Attribute.GetCustomAttribute(p, typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
			string name = (nameAttr?.Name ?? p.Name).ToLower();

			if(Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)) || name == "lazyloader")
				continue;

			Type t = p.PropertyType;
			object? v = p.GetValue(value);

			writer.WritePropertyName(name);
			if((entity = v as Entity) != null) {
				WriteEntity(ref writer, ref value, p);
			} else if((enumerable = v as IEnumerable<Entity>) != null) {
				WriteEntityCollection(ref writer, ref enumerable, p);
			} else {
				JsonSerializer.Serialize(writer, v, t, options);
			}
		}
		writer.WriteEndObject();
	}

	private void WriteEntityCollection(ref Utf8JsonWriter writer, ref IEnumerable<Entity> enumerable, PropertyInfo property) {
		writer.WriteStartArray();
		foreach(Entity entity in enumerable) {
			writer.WriteNumberValue((int)EntityProperty.GetValue(entity)!);
		}
		writer.WriteEndArray();
	}

	private void WriteEntity(ref Utf8JsonWriter writer, ref Entity value, PropertyInfo property) {
		writer.WriteNumberValue((int)EntityProperty.GetValue(value)!);
	}
}