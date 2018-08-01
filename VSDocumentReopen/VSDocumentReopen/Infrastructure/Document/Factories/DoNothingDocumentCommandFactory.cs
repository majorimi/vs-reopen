using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;

namespace VSDocumentReopen.Infrastructure.Document.Factories
{
	public class DoNothingDocumentCommandFactory : IDocumentCommandFactory
	{
		public IDocumentCommand CreateCommand(IClosedDocument closedDocument) => DoNothingCommand.Instance;
	}
}