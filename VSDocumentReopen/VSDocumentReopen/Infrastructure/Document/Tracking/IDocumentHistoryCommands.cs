using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Document.Tracking
{
	public interface IDocumentHistoryCommands
	{
		void Clear();

		void Add(IClosedDocument document);

		IClosedDocument RemoveLast();

		void Remove(IClosedDocument closedDocument);

		void Remove(IEnumerable<IClosedDocument> closedDocuments);

		void Initialize(IEnumerable<IClosedDocument> closedDocuments);
	}
}