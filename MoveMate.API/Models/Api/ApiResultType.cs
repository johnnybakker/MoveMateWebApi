using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;

namespace MoveMate.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApiResultType {
	Success = 0,
	Failed = 1,
}