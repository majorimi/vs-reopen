using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentCommands
{
	public interface IDocumentCommandFactory
	{
		IDocumentCommand CreateCommand(IClosedDocument closedDocument);
	}
}