using VSDocumentReopen.Domain.Documents;
using Xunit;

namespace VSDocumentReopen.Test.Domain.Documents
{
	public class NullDocumentTest
	{
		[Fact]
		public void NullDocument_AllPublicProperties_ShouldNotBeSettable()
		{
			var doc = NullDocument.Instance;

			var props = doc.GetType().GetProperties();

			Assert.All(props, (p) => Assert.False(p.CanWrite));
		}

		[Fact]
		public void NullDocument_AlwaysInvalid()
		{
			var doc = NullDocument.Instance;

			Assert.False(doc.IsValid());
		}

		[Fact]
		public void NullDocument_Singleton()
		{
			var doc = NullDocument.Instance;
			var doc2 = NullDocument.Instance;

			Assert.Equal(doc, doc2);
		}

		[Fact]
		public void NullDocument_PropertiesValues_NotNull()
		{
			var doc = NullDocument.Instance;

			var props = doc.GetType().GetProperties();

			Assert.All(props, (p) => Assert.True(p.GetValue(doc) != null));
		}
	}
}