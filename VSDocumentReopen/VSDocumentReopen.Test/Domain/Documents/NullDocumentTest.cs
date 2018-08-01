using VSDocumentReopen.Domain.Documents;
using Xunit;

namespace VSDocumentReopen.Test.Domain.Documents
{
	public class NullDocumentTest
	{
		[Fact]
		public void AllPublicProperties_ShouldNotBeSettable()
		{
			var doc = NullDocument.Instance;

			var props = doc.GetType().GetProperties();

			Assert.All(props, (p) => Assert.False(p.CanWrite));
		}

		[Fact]
		public void ItShouldBe_Singleton()
		{
			var doc = NullDocument.Instance;
			var doc2 = NullDocument.Instance;

			Assert.Equal(doc, doc2);
		}

		[Fact]
		public void ItShouldBe_AlwaysInvalid()
		{
			var doc = NullDocument.Instance;

			Assert.False(doc.IsValid());
		}
	}
}