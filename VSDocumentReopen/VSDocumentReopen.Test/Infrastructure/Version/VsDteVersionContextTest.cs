using EnvDTE;
using Moq;
using VSDocumentReopen.Infrastructure.Version;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Version
{
	public class VsDteVersionContextTest
	{
		private readonly Mock<_DTE> _dteMock;
		private readonly VsDteVersionContext _vsDteVersionContext;

		public VsDteVersionContextTest()
		{
			_dteMock = new Mock<_DTE>();

			_vsDteVersionContext = new VsDteVersionContext(_dteMock.Object);
		}

		[Fact]
		public void ItShould_Encapsulate_VsDteVersionProvider()
		{
			var provider = _vsDteVersionContext.VersionProvider;

			Assert.IsType<VsDteVersionProvider>(provider);
		}
	}
}