using System;
using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Document.Tracking
{
	public interface IDocumentHistoryQueries
	{
		event EventHandler HistoryChanged;

		int Count { get; }

		IEnumerable<IClosedDocument> Get(int number);

		IEnumerable<IClosedDocument> GetAll();
	}
}