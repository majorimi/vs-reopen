using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.ClosedDocument;
using VSDocumentReopen.Infrastructure.Helpers;

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

			_openSelectedImg.Source = WpfImageSourceConverter.CreateBitmapSource(Properties.Resources.OpenFile_16x);
			_removeSelectedImg.Source = WpfImageSourceConverter.CreateBitmapSource(Properties.Resources.RemoveGuide_16x);
			_clearAllImg.Source = WpfImageSourceConverter.CreateBitmapSource(Properties.Resources.ClearWindowContent_16x);

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

			var count = history.Count();

			_clearAll.IsEnabled = count > 0;
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