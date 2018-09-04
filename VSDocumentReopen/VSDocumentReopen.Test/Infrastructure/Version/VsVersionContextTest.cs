using Moq;
using System;
using VSDocumentReopen.Infrastructure.Version;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Version
{
	public class VsVersionContextTest : IDisposable
	{
		private readonly Mock<VsVersionContext> _versionContextMock;

		public VsVersionContextTest()
		{
			_versionContextMock = new Mock<VsVersionContext>();
		}

		[Fact]
		public void ItShould_Not_Initialized()
		{
			Assert.Null(VsVersionContext.Current);
		}

		[Fact]
		public void ItCouuld_be_Initialized()
		{
			VsVersionContext.Current = _versionContextMock.Object;

			Assert.NotNull(VsVersionContext.Current);
		}

		public void Dispose()
		{
			VsVersionContext.Current = null;
		}
	}
}