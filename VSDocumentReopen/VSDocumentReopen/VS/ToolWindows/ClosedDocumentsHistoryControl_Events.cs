using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;

namespace VSDocumentReopen.VS.ToolWindows
{
	public partial class ClosedDocumentsHistoryControl
	{
		private void _openSelected_Click(object sender, RoutedEventArgs e)
		{
			HandleOperatons(_reopenSomeDocumentsCommandFactory);
		}

		private void _removeSelected_Click(object sender, RoutedEventArgs e)
		{
			HandleOperatons(_removeSomeDocumentsCommandFactory);
		}

		private void _clearAll_Click(object sender, RoutedEventArgs e)
		{
			_clearHistoryCommand.Execute();
		}


		private void _search_TextChanged(object sender, TextChangedEventArgs e)
		{
			var searchText = (e.Source as TextBox)?.Text;

			if (string.IsNullOrWhiteSpace(searchText))
			{
				UpdateHistoryView(GetFullHistory);
			}
			else
			{
				searchText = searchText.ToLower();
				UpdateHistoryView((doc) => doc.FullName.ToLower().Contains(searchText) || doc.Name.ToLower().Contains(searchText));
			}
		}

		private void _listView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (((FrameworkElement)e.OriginalSource).DataContext is ClosedDocumentHistoryItem item)
			{
				var command = _reopenSomeDocumentsCommandFactory.CreateCommand(item.ClosedDocument);
				command.Execute();
			}
		}

		private void _listView_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				HandleOperatons(_removeSomeDocumentsCommandFactory);
			}
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


		private void HandleOperatons(IHistoryCommandFactory historyCommandFactory)
		{
			var selectedItems = _listView.SelectedItems.Cast<ClosedDocumentHistoryItem>()
				.Select(s => s.ClosedDocument);

			var command = historyCommandFactory.CreateCommand(selectedItems.ToArray());
			command.Execute();
		}
	}
}