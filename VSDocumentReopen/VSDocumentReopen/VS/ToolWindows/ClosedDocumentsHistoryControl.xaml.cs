using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.DocumentTracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.ToolWindows.IconHandling;
using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;

namespace VSDocumentReopen.VS.ToolWindows
{
	/// <summary>
	/// Interaction logic for ClosedDocumentsHistoryControl.
	/// </summary>
	public partial class ClosedDocumentsHistoryControl : UserControl
	{
		private class ClosedDocumentHistoryItem
		{
			private static BitmapSource _existsImage = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.FileOK_16x);
			private static BitmapSource _notExistsImage = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.FileError_16x);

			public int Index { get; }

			public bool IsExists { get; }
			public string IsExistsTooltip => IsExists ? "Yes" : "No";
			public BitmapSource IsExistsIcon => IsExists ? _existsImage : _notExistsImage;

			public Image LanguageIcon => null; //TODO: show icon

			public IClosedDocument ClosedDocument { get; }

			public ClosedDocumentHistoryItem(IClosedDocument closedDocument, int index)
			{
				Index = index;
				ClosedDocument = closedDocument;
				IsExists = ClosedDocument.IsValid();
			}
		}

		private readonly Func<IClosedDocument, bool> GetFullHistory = _ => true;

		private readonly IDocumentHistoryQueries _documentHistoryQueries;
		private readonly IHistoryCommand _reopenLastClosdCommand;
		private readonly IHistoryCommandFactory _reopenSomeDocumentsCommandFactory;
		private readonly IHistoryCommandFactory _removeSomeDocumentsCommandFactory;
		private readonly IHistoryCommand _clearHistoryCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistoryControl"/> class.
		/// </summary>
		public ClosedDocumentsHistoryControl(IDocumentHistoryQueries documentHistoryQueries,
			IHistoryCommand reopenLastClosdCommand,
			IHistoryCommandFactory reopenSomeDocumentsCommandFactory,
			IHistoryCommandFactory removeSomeDocumentsCommandFactory,
			IHistoryCommand clearHistoryCommand)
		{
			InitializeComponent();

			_documentHistoryQueries = documentHistoryQueries;
			_reopenLastClosdCommand = reopenLastClosdCommand;
			_reopenSomeDocumentsCommandFactory = reopenSomeDocumentsCommandFactory;
			_removeSomeDocumentsCommandFactory = removeSomeDocumentsCommandFactory;
			_clearHistoryCommand = clearHistoryCommand;

			var openState = new ButtonDisabledState(_openSelected,
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.OpenFile_16x)},
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.OpenFile_16x_Gray)});
			openState.Disable();

			var removeState = new ButtonDisabledState(_removeSelected,
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveGuide_16x)},
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveGuide_16x_Gray)});
			removeState.Disable();

			var clearState = new ButtonDisabledState(_clearAll,
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x)},
				new Image() {Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x_Gray)});
			clearState.Disable();

			//TODO: sort: http://www.wpf-tutorial.com/listview-control/listview-how-to-column-sorting/
			_documentHistoryQueries.HistoryChanged += DocumentHistoryChanged;
			UpdateHistoryView(GetFullHistory);

			_listView.Focus();
		}

		private void DocumentHistoryChanged(object sender, EventArgs e)
		{
			HandleSearch();
		}
	}
}