using VSDocumentReopen.Domain.Documents;
using Xunit;

namespace VSDocumentReopen.Test.Domain.Documents
{
	public class ClosedDocumentTest
	{
		[Fact]
		public void AllPublicProperties_ShouldBeSettable()
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
		public void ItShouldHandle_InvalidPath(string path)
		{
			var doc = new ClosedDocument()
			{
				FullName = path
			};

			Assert.False(doc.IsValid());
		}
	}
}