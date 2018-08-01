using Moq;
using System.Drawing;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.FileIcons;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.FileIcons
{
	public class CachedFileExtensionIconResolverTest
	{
		private readonly CachedFileExtensionIconResolver _fileExtensionIconResolver;
		private readonly Mock<IFileExtensionIconResolver> _fileExtensionResolverMock;

		public CachedFileExtensionIconResolverTest()
		{
			_fileExtensionResolverMock = new Mock<IFileExtensionIconResolver>();

			_fileExtensionIconResolver = new CachedFileExtensionIconResolver(_fileExtensionResolverMock.Object);
		}

		[Fact]
		public void ItShould_Never_ReturnNull()
		{
			_fileExtensionResolverMock.Setup(s => s.GetIcon(It.IsAny<IClosedDocument>()))
				.Returns(() => null);

			var icon = _fileExtensionIconResolver.GetIcon(null);
			var icon2 = _fileExtensionIconResolver.GetIcon(NullDocument.Instance);
			var icon3 = _fileExtensionIconResolver.GetIcon(new ClosedDocument());

			Assert.NotNull(icon);
			Assert.NotNull(icon2);
			Assert.NotNull(icon3);

			_fileExtensionResolverMock.Verify(v => v.GetIcon(It.IsAny<IClosedDocument>()), Times.Never);
		}

		[Fact]
		public void ItShould_Handle_InvalidFileIcon()
		{
			_fileExtensionResolverMock.Setup(s => s.GetIcon(It.IsAny<IClosedDocument>()))
				.Returns(() => null);

			var icon = _fileExtensionIconResolver.GetIcon(new ClosedDocument()
			{
				FullName = "c/test.cs"
			});

			Assert.NotNull(icon);

			_fileExtensionResolverMock.Verify(v => v.GetIcon(It.IsAny<IClosedDocument>()), Times.Once);
		}

		[Fact]
		public void ItShould_Resolve_IconFromCache()
		{
			var ico = Icon.FromHandle(Resources.FileError_16x.GetHbitmap());
			_fileExtensionResolverMock.Setup(s => s.GetIcon(It.IsAny<IClosedDocument>()))
				.Returns(() => ico);

			var icon = _fileExtensionIconResolver.GetIcon(new ClosedDocument()
			{
				FullName = "c/test.xml"
			});
			var icon2 = _fileExtensionIconResolver.GetIcon(new ClosedDocument()
			{
				FullName = "c/doc.xml"
			});

			Assert.NotNull(icon);
			Assert.NotNull(icon2);
			Assert.Same(ico, icon);
			Assert.Same(ico, icon2);

			_fileExtensionResolverMock.Verify(v => v.GetIcon(It.IsAny<IClosedDocument>()), Times.Once);
		}
	}
}