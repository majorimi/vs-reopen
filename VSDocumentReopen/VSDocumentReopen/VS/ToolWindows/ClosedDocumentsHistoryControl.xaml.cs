using System;
using System.Windows.Controls;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.ClosedDocument;
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
			private readonly IClosedDocument _closedDocument;

			public int Index { get; }
			public bool IsExists => _closedDocument.IsValid();

			public DateTime ClosedAt => _closedDocument.ClosedAt;
			public string FullName => _closedDocument.FullName;
			public string Name => _closedDocument.Name;

			public string Type => _closedDocument.Language;
			public Image Icon => null;

			public ClosedDocumentHistoryItem(IClosedDocument closedDocument, int index)
			{
				Index = index;
				_closedDocument = closedDocument;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistoryControl"/> class.
		/// </summary>
		public ClosedDocumentsHistoryControl()
		{
			InitializeComponent();

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

			_listView.Focus();

			//Sort: http://www.wpf-tutorial.com/listview-control/listview-how-to-column-sorting/
			DocumentHistoryManager.Instance.HistoryChanged += DocumentHistoryChanged;
			UpdateHistoryView(GetFullHistory);
		}

		private void DocumentHistoryChanged(object sender, EventArgs e)
		{
			UpdateHistoryView(GetFullHistory);
		}

		private void UpdateHistoryView(Func<IClosedDocument, bool> documentFilter)
		{
			_listView.Items.Clear();

			var history = DocumentHistoryManager.Instance.GetAll();
			var i = 1;

			foreach (var doc in history)
			{
				if (documentFilter(doc))
				{
					_listView.Items.Add(new ClosedDocumentHistoryItem(doc, i));
				}

				i++;
			}

			var count = i - 1;

			if (count > 0)
			{
				_clearAll.GetImageButtonState().Enable();
			}
			else
			{
				_clearAll.GetImageButtonState().Disable();
			}

			_numberOfItems.Content = _listView.Items.Count == count
				? count.ToString()
				: $"{_listView.Items.Count}/{count}";
		}

		private bool GetFullHistory(IClosedDocument document)
		{
			return true;
		}
	}
}