using System;
using System.Collections.Generic;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentTracking
{
	public interface IDocumentHistoryQueries
	{
		event EventHandler HistoryChanged;

		IEnumerable<IClosedDocument> Get(int number);

		IEnumerable<IClosedDocument> GetAll();
	}
}