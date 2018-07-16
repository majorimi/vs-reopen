using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows
{
	/// <summary>
	/// Interaction logic for ClosedDocumentsHistoryControl.
	/// </summary>
	public partial class ClosedDocumentsHistoryControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistoryControl"/> class.
		/// </summary>
		public ClosedDocumentsHistoryControl()
		{
			InitializeComponent();

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

			foreach (var doc in Infrastructure.ClosedDocument.DocumentHistory.Instance.GetAll())
			{
				_listView.Items.Add(doc);
			}
		}
	}
}