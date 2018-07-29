using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;

namespace VSDocumentReopen.Infrastructure.Document.Factories
{
	public interface IDocumentCommandFactory
	{
		IDocumentCommand CreateCommand(IClosedDocument closedDocument);
	}
}