using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSDocumentReopen.Infrastructure.ClosedDocument;
using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;

namespace VSDocumentReopen.VS.ToolWindows
{
	public partial class ClosedDocumentsHistoryControl
	{
		private void _listView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (((FrameworkElement)e.OriginalSource).DataContext is ClosedDocumentHistoryItem item)
			{
				HandleOperatons(new List<ClosedDocumentHistoryItem>() { item });
			}
		}

		private void _listView_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				//HandleOperatons(_listView.SelectedItems);
			}
		}


		private void _search_TextChanged(object sender, TextChangedEventArgs e)
		{
			var searchText = (e.Source as TextBox)?.Text;

			if(string.IsNullOrWhiteSpace(searchText))
			{
				UpdateHistoryView(GetFullHistory);
			}
			else
			{
				searchText = searchText.ToLower();
				UpdateHistoryView((doc) => doc.FullName.ToLower().Contains(searchText) || doc.Name.ToLower().Contains(searchText));
			}
		}


		private void _openSelected_Click(object sender, RoutedEventArgs e)
		{
			//HandleOperatons(_listView.SelectedItems);
		}

		private void _removeSelected_Click(object sender, RoutedEventArgs e)
		{
			//HandleOperatons(_listView.SelectedItems);
		}

		private void _clearAll_Click(object sender, RoutedEventArgs e)
		{
			DocumentHistoryManager.Instance.Clear();
		}


		private void _listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (_listView.SelectedItems.Count > 0)
			{
				_openSelected.GetImageButtonState().Enable();
				_removeSelected.GetImageButtonState().Enable();
				return;
			}

			_openSelected.GetImageButtonState().Disable();
			_removeSelected.GetImageButtonState().Disable();
		}

		private void HandleOperatons(IEnumerable<ClosedDocumentHistoryItem> selectedItems) //TODO: add CommandFactory
		{

		}
	}
}