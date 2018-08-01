using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.FileIcons;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.FileIcons
{
	public class WindowsFileExtensionIconResolverTest
	{
		private readonly WindowsFileExtensionIconResolver _fileExtensionIconResolver;

		public WindowsFileExtensionIconResolverTest()
		{
			_fileExtensionIconResolver = new WindowsFileExtensionIconResolver();
		}

		[Fact]
		public void ItShouldHandle_NullDocument()
		{
			var icon = _fileExtensionIconResolver.GetIcon(NullDocument.Instance);

			Assert.Null(icon);
		}

		[Fact]
		public void ItShouldHandle_DocumentDoesNotExist()
		{
			var icon = _fileExtensionIconResolver.GetIcon(new ClosedDocument());

			Assert.Null(icon);
		}
	}
}