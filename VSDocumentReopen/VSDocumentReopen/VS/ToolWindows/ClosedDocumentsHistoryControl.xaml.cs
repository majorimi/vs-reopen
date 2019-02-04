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

		private readonly IDictionary<int, string> _columnHeaders;
		private readonly IList<GridViewColumn> _hiddenColumns = new List<GridViewColumn>();
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

			var removeNonExistingState = new ButtonDisabledState(_removeNonExisting,
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveNonExisting_16x) },
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.RemoveNonExisting_16x_Gray) });
			removeNonExistingState.Disable();

			var clearState = new ButtonDisabledState(_clearAll,
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x) },
				new Image() { Source = WpfImageSourceConverter.CreateBitmapSource(VSDocumentReopen.Resources.ClearWindowContent_16x_Gray) });
			clearState.Disable();

			_documentHistoryQueries.HistoryChanged += DocumentHistoryChanged;
			UpdateHistoryView(GetFullHistory);

			_listView.Focus();
			var customizationSettings = LoadCustomizationSettings();

			_columnHeaders = Enumerable.Range(0, _listViewContect.Columns.Count).ToDictionary(i => i++,
				i => _listViewContect.Columns[i].GetGridViewHeaderText());

			AddContextMenu(customizationSettings);
			HandleColumnsStatus(customizationSettings);
		}

		private void HandleColumnsStatus(HistoryControlData historyControlData)
		{
			if(historyControlData?.ColumnsInfo?.Any() ?? false)
			{
				var hiddenColumns = historyControlData.ColumnsInfo.Where(x => !x.Visible);
				foreach (var item in hiddenColumns)
				{
					if (_columnHeaders.ContainsKey(item.Id))
					{
						HideColumn(_columnHeaders[item.Id]);
					}
				}

				var visibleColumns = historyControlData.ColumnsInfo.Except(hiddenColumns).OrderBy(o => o.Position);
				foreach (var item in visibleColumns)
				{
					if (_columnHeaders.ContainsKey(item.Id))
					{
						SetColumnData(_columnHeaders[item.Id], item.Position, item.Width);
					}
				}
			}
		}

		private void AddContextMenu(HistoryControlData historyControlData)
		{
			var contextMenu = new ContextMenu();

			var removeSortingMenu = new MenuItem()
			{
				Uid = "_removeSortingMenu",
				Header = "Remove sorting",
				IsCheckable = false,
			};
			removeSortingMenu.Click += _listViewRemoveSort_Click;

			var hideShowColumnMenu = new MenuItem()
			{
				Uid = "_showHideColumnsMenu",
				Header = "Show/Hide columns",
				IsCheckable = false,
			};
			hideShowColumnMenu.Click += _listViewShowColumns_Click;

			int index = 0;
			foreach (var item in _listViewContect.Columns)
			{
				var setting = historyControlData?.ColumnsInfo?.SingleOrDefault(x => x.Id == index);

				var menu = new MenuItem()
				{
					Uid = index++.ToString(),
					Header = item.GetGridViewHeaderText(),
					IsCheckable = true,
					IsChecked = setting?.Visible ?? true
				};
				hideShowColumnMenu.Items.Add(menu);
			}

			var resetShowColumnMenu = new MenuItem()
			{
				Uid = "_resetShowColumnMenu",
				Header = "Reset column customization",
				IsCheckable = false,
			};
			resetShowColumnMenu.Click += _resetShowColumnMenu_Click;

			contextMenu.Items.Add(removeSortingMenu);
			contextMenu.Items.Add(hideShowColumnMenu);
			contextMenu.Items.Add(resetShowColumnMenu);

			_listView.ContextMenu = contextMenu;
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

		private HistoryControlData LoadCustomizationSettings()
		{
			var settingsRepository = _historyToolWindowRepositoryFactory.Create();
			var settings = settingsRepository?.GetSettings();

			_search.HistoryList = settings?.SearchHistory?.ToList() ?? new List<string>();

			return settings;
		}

		private void SaveCustomizationSettings()
		{
			var settingsRepository = _historyToolWindowRepositoryFactory.Create();

			var columnsInfo = new List<ColumnInfo>();
			foreach (var item in _columnHeaders)
			{
				var column = _listViewContect.Columns.SingleOrDefault(x => x.GetGridViewHeaderText() == item.Value);
				columnsInfo.Add(new ColumnInfo()
				{
					Id = item.Key,
					Position = column != null ? _listViewContect.Columns.IndexOf(column) : -1,
					Visible = column != null,
					Width = column?.Width ?? 0
				});
			}

			var settings = new HistoryControlData()
			{
				SearchHistory = _search.HistoryList.Take(10),
				ColumnsInfo = columnsInfo
			};

			settingsRepository?.SaveSettings(settings);
		}

		public void Dispose()
		{
			SaveCustomizationSettings();
		}
	}
}