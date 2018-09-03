using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Domain.HistoryControl;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.FileIcons;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.ToolWindows.IconHandling;
using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;

namespace VSDocumentReopen.VS.ToolWindows
{
	/// <summary>
	/// Interaction logic for ClosedDocumentsHistoryControl.
	/// </summary>
	public partial class ClosedDocumentsHistoryControl : UserControl, IDisposable
	{
		private static readonly Dictionary<string, BitmapSource> _fileTypeImages = new Dictionary<string, BitmapSource>();

		private readonly Func<IClosedDocument, bool> GetFullHistory = _ => true;

		private readonly IDocumentHistoryQueries _documentHistoryQueries;
		private readonly IHistoryCommand _reopenLastClosdCommand;
		private readonly IHistoryCommandFactory _reopenSomeDocumentsCommandFactory;
		private readonly IHistoryCommandFactory _removeSomeDocumentsCommandFactory;
		private readonly IHistoryCommand _clearHistoryCommand;
		private readonly IFileExtensionIconResolver _fileExtensionIconResolver;
		private readonly IHistoryToolWindowRepositoryFactory _historyToolWindowRepositoryFactory;
		private GridViewColumnHeader listViewSortCol = null;
		private SortAdorner listViewSortAdorner = null;

		public bool ContextMenuVisible => listViewSortCol != null;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistoryControl"/> class.
		/// </summary>
		public ClosedDocumentsHistoryControl(IDocumentHistoryQueries documentHistoryQueries,
			IHistoryCommand reopenLastClosdCommand,
			IHistoryCommandFactory reopenSomeDocumentsCommandFactory,
			IHistoryCommandFactory removeSomeDocumentsCommandFactory,
			IHistoryCommand clearHistoryCommand,
			IFileExtensionIconResolver fileExtensionIconResolver,
			IHistoryToolWindowRepositoryFactory historyToolWindowRepositoryFactory)
		{
			InitializeComponent();

			_documentHistoryQueries = documentHistoryQueries;
			_reopenLastClosdCommand = reopenLastClosdCommand;
			_reopenSomeDocumentsCommandFactory = reopenSomeDocumentsCommandFactory;
			_removeSomeDocumentsCommandFactory = removeSomeDocumentsCommandFactory;
			_clearHistoryCommand = clearHistoryCommand;
			_fileExtensionIconResolver = fileExtensionIconResolver;
			_historyToolWindowRepositoryFactory = historyToolWindowRepositoryFactory;

			var openState = new ButtonDisabledState(_openSelected,
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.OpenFile_16x) },
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.OpenFile_16x_Gray) });
			openState.Disable();

			var removeState = new ButtonDisabledState(_removeSelected,
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveGuide_16x) },
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveGuide_16x_Gray) });
			removeState.Disable();

			var clearState = new ButtonDisabledState(_clearAll,
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x) },
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x_Gray) });
			clearState.Disable();

			_documentHistoryQueries.HistoryChanged += DocumentHistoryChanged;
			UpdateHistoryView(GetFullHistory);

			_listView.Focus();
			LoadSettings();
		}

		private void DocumentHistoryChanged(object sender, EventArgs e)
		{
			HandleSearch();
		}

		private BitmapSource GetFileTypeBitmapSource(IClosedDocument doc)
		{
			var extension = Path.GetExtension(doc.FullName).ToLower();

			if (!_fileTypeImages.ContainsKey(extension))
			{
				var bitmapSource = WpfImageSourceConverter.CreateBitmapSource(_fileExtensionIconResolver.GetIcon(doc).ToBitmap());

				_fileTypeImages.Add(extension, bitmapSource);
				return bitmapSource;
			}

			return _fileTypeImages[extension];
		}

		private void LoadSettings()
		{
			var settingsRepository = _historyToolWindowRepositoryFactory.Create();
			var settings = settingsRepository?.GetSettings();

			_search.HistoryList = settings?.SearchHistory?.ToList() ?? new List<string>();
		}

		public void Dispose()
		{
			var settingsRepository = _historyToolWindowRepositoryFactory.Create();

			var settings = new HistoryControlData()
			{
				SearchHistory = _search.HistoryList.Take(10),
			};

			settingsRepository?.SaveSettings(settings);
		}
	}
}