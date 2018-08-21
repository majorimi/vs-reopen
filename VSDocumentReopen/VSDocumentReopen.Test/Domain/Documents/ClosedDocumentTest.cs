using System;
using VSDocumentReopen.Domain.Documents;
using Xunit;

namespace VSDocumentReopen.Test.Domain.Documents
{
	public class ClosedDocumentTest
	{
		[Fact]
		public void ClosedDocument_AllPublicProperties_ShouldBeSettable()
		{
			var doc = new ClosedDocument();

			var props = doc.GetType().GetProperties();

			Assert.All(props, (p) => Assert.True(p.CanWrite));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("invalid")]
		public void ClosedDocument_Handle_InvalidPath(string path)
		{
			var doc = new ClosedDocument()
			{
				FullName = path
			};

			Assert.False(doc.IsValid());
		}

		[Fact]
		public void ClosedDocument_ToString_Should_Contins_Data()
		{
			var doc = new ClosedDocument()
			{
				FullName = "c:\\test.cs",
				ClosedAt = DateTime.Now
			};

			var str = doc.ToString();

			Assert.Contains(doc.FullName, str);
			Assert.Contains(doc.ClosedAt.ToString(), str);
		}
	}
}