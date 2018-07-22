using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.HistoryCommands
{
	public interface IHistoryCommandFactory
	{
		IHistoryCommand CreateCommand(params IClosedDocument[] closedDocuments);
	}
}