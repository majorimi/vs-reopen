using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows
{
	public static class GridViewExtensions
	{
		public static string GetGridViewHeaderText(this GridViewColumn column)
		{
			return (column.Header as GridViewColumnHeader)?.Content?.ToString().Trim();
		}
	}
}