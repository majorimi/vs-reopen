using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentTracking
{
	public interface IDocumentHistoryCommands
	{
		void Clear();

		void Add(ClosedDocument document);

		IClosedDocument RemoveLast();

		void Remove(IClosedDocument closedDocument);

		void Remove(IEnumerable<IClosedDocument> closedDocuments);

		void Initialize(IEnumerable<IClosedDocument> closedDocuments);
	}
}