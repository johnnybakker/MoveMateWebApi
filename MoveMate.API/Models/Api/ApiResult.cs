using MoveMate.Models.Api;
namespace MoveMate.Models;

public class ApiResult {

	public ApiResultType Type { get; set; } = ApiResultType.Success;
	public object? Data { get; set; } = null;
	
	public static ApiResult From<T>(ApiResultType type, T data) => new ApiResult { Type = type,  Data = data };
	public static ApiResult Success<T>(T data) => From(ApiResultType.Success, data);
	public static ApiResult Failed<T>(T data) => From(ApiResultType.Failed, data);
	public static ApiResult Success() => From(ApiResultType.Success);
	public static ApiResult Failed() => From(ApiResultType.Failed);
	public static ApiResult From(ApiResultType type) => new ApiResult { Type = type, Data = null };

	public bool Unpack<T>(out T? o)
	{
		try
		{
			o = (T?)Convert.ChangeType(Data, typeof(T));
			return true;
		} 
		catch { 
			o = default;
			return false;
		}
	}
}

