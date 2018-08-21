using VSDocumentReopen.Domain.Documents;
using Xunit;

namespace VSDocumentReopen.Test.Domain.Documents
{
	public class ClosedDocumentComparerTest
	{
		private readonly ClosedDocumentComparer _closedDocumentComparer;

		public ClosedDocumentComparerTest()
		{
			_closedDocumentComparer = new ClosedDocumentComparer();
		}

		[Fact]
		public void Equals_Should_Handle_Null()
		{
			var ret = _closedDocumentComparer.Equals(null, null);

			Assert.True(ret);
		}

		[Fact]
		public void Equals_Should_Handle_NullDocument()
		{
			var ret = _closedDocumentComparer.Equals(NullDocument.Instance, NullDocument.Instance);

			Assert.True(ret);
		}

		[Fact]
		public void Equals_Should_Compare_Documents_By_FullName()
		{
			var ret = _closedDocumentComparer.Equals(NullDocument.Instance, new ClosedDocument() { FullName = null });

			Assert.False(ret);
		}

		[Fact]
		public void Equals_Should_Compare_Documents_By_FullName2()
		{
			var ret = _closedDocumentComparer.Equals(NullDocument.Instance, new ClosedDocument() { FullName = "" });

			Assert.True(ret);
		}

		[Fact]
		public void Equals_Should_Compare_Documents_By_FullName3()
		{
			var ret = _closedDocumentComparer.Equals(new ClosedDocument() { FullName = "c:\\test.cs" }, new ClosedDocument() {FullName = "c:\\test.cs"});

			Assert.True(ret);
		}

		[Fact]
		public void Equals_Should_Compare_Documents_By_FullName4()
		{
			var ret = _closedDocumentComparer.Equals(new ClosedDocument() { FullName = "c:\\test.cs" }, new ClosedDocument() { FullName = "c:\\test2.cs" });

			Assert.False(ret);
		}

		[Fact]
		public void GetHashCode_Should_Handle_Null()
		{
			var ret = _closedDocumentComparer.GetHashCode(null);

			Assert.Equal(0, ret);
		}

		[Fact]
		public void GetHashCode_Should_Handle_NullDocument()
		{
			var ret = _closedDocumentComparer.GetHashCode(NullDocument.Instance);

			Assert.NotEqual(0, ret);
		}

		[Fact]
		public void GetHashCode_Should_Handle_Null_FullName()
		{
			var ret = _closedDocumentComparer.GetHashCode(new ClosedDocument() {FullName = null});

			Assert.Equal(0, ret);
		}

		[Fact]
		public void GetHashCode_Should_Handle_FullName()
		{
			var ret = _closedDocumentComparer.GetHashCode(new ClosedDocument() { FullName = "c:\\test.cs" });

			Assert.NotEqual(0, ret);
		}
	}
}