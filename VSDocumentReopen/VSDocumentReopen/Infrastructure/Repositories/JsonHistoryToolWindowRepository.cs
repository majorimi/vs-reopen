using VSDocumentReopen.Domain.HistoryControl;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public sealed class JsonHistoryToolWindowRepository : JsonHistoryRepositoryBase<HistoryControlData>, IHistoryToolWindowRepository
	{
		public JsonHistoryToolWindowRepository(IJsonSerializer serializer, string storageFile)
			: base(serializer, storageFile)
		{}

		public HistoryControlData GetSettings()
		{
			return Load();
		}

		public bool SaveSettings(HistoryControlData data)
		{
			return Save(data);
		}
	}
}