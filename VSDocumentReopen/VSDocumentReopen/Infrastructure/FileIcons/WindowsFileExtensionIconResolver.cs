using System.Drawing;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public class WindowsFileExtensionIconResolver : IFileExtensionIconResolver
	{
		public Bitmap GetIcon(IClosedDocument document)
		{
			if (document.IsValid())
			{
				var iconForFile = Icon.ExtractAssociatedIcon(document.FullName);
				return iconForFile.ToBitmap();
			}

			return null;
		}
	}
}
