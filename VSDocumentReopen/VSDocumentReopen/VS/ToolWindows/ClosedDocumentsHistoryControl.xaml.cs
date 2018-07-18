using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSDocumentReopen.Domain.Documents;

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

			//Sort: http://www.wpf-tutorial.com/listview-control/listview-how-to-column-sorting/
			Infrastructure.ClosedDocument.DocumentHistory.Instance.HistoryChanged += DocumentHistoryChanged;
			RefreshView();
		}

		private void DocumentHistoryChanged(object sender, System.EventArgs e)
		{
			RefreshView();
		}

		private void RefreshView()
		{
			_listView.Items.Clear();

			var i = 1;
			foreach (var doc in Infrastructure.ClosedDocument.DocumentHistory.Instance.GetAll())
			{
				_listView.Items.Add(new ClosedDocumentHistoryItem(doc, i++));
			}

			_numberOfItems.Content = _listView.Items.Count;
		}

		private void _listView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (((FrameworkElement)e.OriginalSource).DataContext is ClosedDocumentHistoryItem item)
			{

			}
		}

		private void _listView_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				var selectedItems = _listView.SelectedItems;
			}
		}
	}
}