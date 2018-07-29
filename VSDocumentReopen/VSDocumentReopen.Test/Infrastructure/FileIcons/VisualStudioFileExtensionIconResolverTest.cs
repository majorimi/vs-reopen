using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using VSDocumentReopen.Infrastructure.FileIcons;
using Moq;
using Microsoft.VisualStudio.Shell.Interop;
using VSDocumentReopen.Domain.Documents;
using Xunit;
using Microsoft.Internal.VisualStudio.PlatformUI;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Test.Infrastructure.FileIcons
{
	public class VisualStudioFileExtensionIconResolverTest
	{
		private readonly VisualStudioFileExtensionIconResolver _fileExtensionIconResolver;
		private readonly Mock<IVsImageService2> _iVsImageService2Mock;

		public VisualStudioFileExtensionIconResolverTest()
		{
			_iVsImageService2Mock = new Mock<IVsImageService2>();
			_iVsImageService2Mock.Setup(s => s.GetIconForFile(It.Is<string>(o => string.IsNullOrWhiteSpace(o)), It.IsAny<__VSUIDATAFORMAT>()))
				.Returns(() => null);

			_fileExtensionIconResolver = new VisualStudioFileExtensionIconResolver(_iVsImageService2Mock.Object);
		}

		[Fact]
		public void ItShouldHandle_Null()
		{
			var icon = _fileExtensionIconResolver.GetIcon(null);

			Assert.Null(icon);
			_iVsImageService2Mock.Verify(v => v.GetIconForFile(It.IsAny<string>(), It.IsAny<__VSUIDATAFORMAT>()), Times.Never);
		}

		[Fact]
		public void ItShouldHandle_NullDocument()
		{
			var icon = _fileExtensionIconResolver.GetIcon(NullDocument.Instance);

			Assert.Null(icon);
			_iVsImageService2Mock.Verify(v => v.GetIconForFile(It.IsAny<string>(), It.IsAny<__VSUIDATAFORMAT>()), Times.Never);
		}

		[Fact]
		public void ItShouldHandle_DocumentDoesNotExist()
		{
			var icon = _fileExtensionIconResolver.GetIcon(new ClosedDocument()
			{
				Name = null
			});

			Assert.Null(icon);
			_iVsImageService2Mock.Verify(v => v.GetIconForFile(It.IsAny<string>(), It.IsAny<__VSUIDATAFORMAT>()), Times.Never);
		}

		[Fact]
		public void ItShouldHandle_Document()
		{
			var ico = Icon.FromHandle(Resources.FileError_16x.GetHbitmap());
			_iVsImageService2Mock.Setup(s => s.GetIconForFile(It.Is<string>(o => o == "test.cs"), It.IsAny<__VSUIDATAFORMAT>()))
				.Returns(() => new WinFormsIconUIObject(ico));

			var icon = _fileExtensionIconResolver.GetIcon(new ClosedDocument()
			{
				FullName = "c:/test.cs",
				Name = "test.cs"
			});

			Assert.NotNull(icon);
			Assert.Same(ico, icon);
			_iVsImageService2Mock.Verify(v => v.GetIconForFile(It.Is<string>(o => o == "test.cs"), It.IsAny<__VSUIDATAFORMAT>()), Times.Once);
		}
	}
}