using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public interface IHistoryRepository
	{
		bool SaveHistory(IEnumerable<IClosedDocument> closedDocumentHistories);

		IEnumerable<IClosedDocument> GetHistory();
	}
}