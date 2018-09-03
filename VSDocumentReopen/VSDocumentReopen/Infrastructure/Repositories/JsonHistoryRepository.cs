using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public sealed class JsonHistoryRepository : JsonHistoryRepositoryBase<IEnumerable<IClosedDocument>>, IHistoryRepository
	{
		public JsonHistoryRepository(IJsonSerializer serializer, string storageFile)
			: base(serializer, storageFile)
		{}

		public bool SaveHistory(IEnumerable<IClosedDocument> closedDocumentHistories)
		{
			return Save(closedDocumentHistories);
		}

		public IEnumerable<IClosedDocument> GetHistory()
		{
			return Load();
		}
	}
}