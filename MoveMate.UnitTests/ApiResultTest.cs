using System.Runtime.Intrinsics.Arm;
using MoveMate.Models;
using MoveMate.Models.Api;

namespace MoveMate.UnitTests;


public class ApiResultTest {


	[Fact]
	public void TestIntegerResult() {

		int intResult;
		string? stringResult;
		bool boolResult;
		var success = ApiResult.Success(1);
		var failed = ApiResult.Failed(0);
		
		Assert.Equal(ApiResultType.Success, success.Type);
		Assert.Equal(ApiResultType.Failed, failed.Type);

		Assert.True(success.Unpack(out intResult));
		Assert.Equal(1, intResult);

		Assert.True(failed.Unpack(out intResult));
		Assert.Equal(0, intResult);

		Assert.True(success.Unpack(out stringResult));
		Assert.NotNull(stringResult);
		Assert.Equal("1", stringResult);

		Assert.True(success.Unpack(out boolResult));
		Assert.True(boolResult);

		Assert.True(failed.Unpack(out boolResult));
		Assert.False(boolResult);
	}


}