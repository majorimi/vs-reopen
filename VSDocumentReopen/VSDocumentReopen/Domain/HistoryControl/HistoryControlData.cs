using System.Collections.Generic;

namespace VSDocumentReopen.Domain.HistoryControl
{
	public class HistoryControlData
	{
		public IEnumerable<string> SearchHistory { get; set; }

		public IEnumerable<ColumnInfo> ColumnsInfo { get; set; }
	}
}