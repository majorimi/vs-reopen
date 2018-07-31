using System.Windows.Media.Imaging;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.VS.ToolWindows.IconHandling;

namespace VSDocumentReopen.VS.ToolWindows
{
	public partial class ClosedDocumentsHistoryControl
	{
		private class ClosedDocumentHistoryItem
		{
			private static BitmapSource _existsImage = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.FileOK_16x);
			private static BitmapSource _notExistsImage = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.FileError_16x);

			public int Index { get; }

			public bool IsExists { get; }
			public string IsExistsTooltip => IsExists ? "Yes" : "No";
			public BitmapSource IsExistsIcon => IsExists ? _existsImage : _notExistsImage;

			public BitmapSource LanguageIcon { get; }

			public IClosedDocument ClosedDocument { get; }

			public ClosedDocumentHistoryItem(IClosedDocument closedDocument, int index, BitmapSource typeIcon)
			{
				Index = index;
				ClosedDocument = closedDocument;
				IsExists = ClosedDocument.IsValid();
				LanguageIcon = typeIcon;
			}
		}
	}
}