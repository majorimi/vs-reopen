using System;
using System.Windows.Controls;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.ClosedDocument;
using VSDocumentReopen.Infrastructure.Commands;
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
			public int Index { get; }
			public bool IsExists => ClosedDocument.IsValid();
			public Image Icon => null; //TODO: show icon

			public IClosedDocument ClosedDocument { get; }

			public ClosedDocumentHistoryItem(IClosedDocument closedDocument, int index)
			{
				Index = index;
				ClosedDocument = closedDocument;
			}
		}

		private readonly Func<IClosedDocument, bool> GetFullHistory = _ => true;
		private readonly IDocumentCommandFactory _documentCommandFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistoryControl"/> class.
		/// </summary>
		public ClosedDocumentsHistoryControl(IDocumentCommandFactory documentCommandFactory)
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

			//TODO: sort: http://www.wpf-tutorial.com/listview-control/listview-how-to-column-sorting/
			DocumentHistoryManager.Instance.HistoryChanged += DocumentHistoryChanged;
			UpdateHistoryView(GetFullHistory);
			_documentCommandFactory = documentCommandFactory;
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
	}
}