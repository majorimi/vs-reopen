using EnvDTE;
using Moq;
using VSDocumentReopen.Infrastructure.Version;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Version
{
	public class VsDteVersionProviderTest
	{
		private readonly Mock<_DTE> _dteMock;
		private readonly VsDteVersionProvider _vsDteVersionProvider;

		public VsDteVersionProviderTest()
		{
			_dteMock = new Mock<_DTE>();

			_vsDteVersionProvider = new VsDteVersionProvider(_dteMock.Object);
		}

		[Fact]
		public void ItShould_AcceptNull()
		{
			var provider = new VsDteVersionProvider(null);

			Assert.Null(provider.GetVersion());
			_dteMock.VerifyGet(g => g.Version, Times.Never);
		}

		[Fact]
		public void ItSould_Return_Version_From_DTE()
		{
			_dteMock.SetupGet(g => g.Version).Returns("15");

			var version = _vsDteVersionProvider.GetVersion();

			Assert.NotNull(version);
			Assert.Equal("15", version);
			_dteMock.VerifyGet(g => g.Version, Times.Once);
		}
	}
}