using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VSDocumentReopen.VS.ToolWindows
{
	public partial class ClosedDocumentsHistoryControl
	{
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


		private void _search_TextChanged(object sender, TextChangedEventArgs e)
		{

		}


		private void _openSelected_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void _removeSelected_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void _clearAll_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}
	}
}