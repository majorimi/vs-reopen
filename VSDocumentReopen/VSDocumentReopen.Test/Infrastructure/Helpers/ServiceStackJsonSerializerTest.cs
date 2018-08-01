using System;
using System.Collections;
using System.Collections.Generic;
using VSDocumentReopen.Infrastructure.Helpers;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Helpers
{
	public class ServiceStackJsonSerializerTest : IDisposable
	{
		private class TestObj
		{
			public int Id { get; set; }
			public string Data { get; set; }
		}
		private class TestObjData : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				yield return new object[] { 0, "" };
				yield return new object[] { 10, "0123456789!@#$%^&*()_+asdfghjkl;'qwertyuiop[]zxcvbnm,./" };
				yield return new object[] { 9999, "teeeeeest" };
			}

			IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
		}

		private readonly ServiceStackJsonSerializer _serviceStackJsonSerializer;

		public ServiceStackJsonSerializerTest()
		{
			_serviceStackJsonSerializer = new ServiceStackJsonSerializer();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData(5)]
		public void Serialize_ShouldHandle_InvalidObjects(object obj)
		{
			var ret =_serviceStackJsonSerializer.Serialize<object>(obj);

			Assert.Equal(ret?.Trim('"'), obj?.ToString());
		}

		[Theory]
		[ClassData(typeof(TestObjData))]
		public void Serialize_ShouldHandle_ValidObjects(int id, string data)
		{
			var ret = _serviceStackJsonSerializer.Serialize<TestObj>(new TestObj()
			{
				Id = id,
				Data = data
			});

			Assert.Equal($"{{\"Id\":{id},\"Data\":\"{data}\"}}", ret);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("  ")]
		[InlineData("5")]
		public void Deserialize_ShouldHandle_InvalidString(string json)
		{
			var ret = _serviceStackJsonSerializer.Deserialize<object>(json);

			Assert.Equal(json, ret?.ToString());
		}

		[Fact]
		public void Deserialize_ShouldHandle_EmptyString()
		{
			var ret = _serviceStackJsonSerializer.Deserialize<object>("");

			Assert.Null(ret);
		}

		[Theory]
		[InlineData(0, "")]
		[InlineData(15, "test")]
		[InlineData(99999, "0123456789!@#$%^&*()_+asdfghjkl;'qwertyuiop[]zxcvbnm,./")]
		public void Deserialize_ShouldHandle_ValidJson(int id, string data)
		{
			var ret = _serviceStackJsonSerializer.Deserialize<TestObj>($"{{\"Id\":{id},\"Data\":\"{data}\"}}");

			Assert.NotNull(ret);
			Assert.Equal(id, ret.Id);
			Assert.Equal(data, ret.Data);
		}

		public void Dispose()
		{
		}
	}
}