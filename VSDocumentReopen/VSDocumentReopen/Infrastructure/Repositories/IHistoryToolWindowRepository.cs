using VSDocumentReopen.Domain.HistoryControl;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public interface IHistoryToolWindowRepository
	{
		HistoryControlData GetSettings();

		bool SaveSettings(HistoryControlData data);
	}
}