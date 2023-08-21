using MoveMateWebApi.Models.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MoveMateWebApi.Models;

public class ApiResult {

	[JsonConverter(typeof(StringEnumConverter))]
	public ApiResultType Type { get; set; } = ApiResultType.Success;
	public JToken? Data { get; set; } = null;

	public static ApiResult From<T>(ApiResultType type, T? data) where T : notnull => new ApiResult { 
		Type = type,  
		Data = data == null ? null : JToken.FromObject(data)
	};

	public static ApiResult Success() => From<object>(ApiResultType.Success, null);
	public static ApiResult Success<T>(T? data) where T : notnull => From<T>(ApiResultType.Success, data);
	public static ApiResult Failed() => From<object>(ApiResultType.Failed, null);
	public static ApiResult Failed<T>(T? data) where T : notnull => From<T>(ApiResultType.Failed, data);
}



