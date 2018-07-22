using System;
using VSDocumentReopen.Infrastructure.Helpers;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Helpers
{
	public class ServiceStackJsonSerializerTest : IDisposable
	{
		private class TestData
		{
			public int Id { get; set; }
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

		[Fact]
		public void Serialize_ShouldHandle_ValidObjects()
		{
			var ret = _serviceStackJsonSerializer.Serialize<TestData>(new TestData()
			{

			});

			Assert.Equal("{\"Id\":0}", ret);
		}

		[Theory]
		[InlineData(null)]
		//[InlineData("")]
		[InlineData("  ")]
		[InlineData("5")]
		public void Deserialize_ShouldHandle_InvalidObjects(string json)
		{
			var ret = _serviceStackJsonSerializer.Deserialize<object>(json);

			Assert.Equal(json, ret?.ToString());
		}

		public void Dispose()
		{
		}
	}
}