using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentCommands
{
	public class DoNothingDocumentCommandFactory : IDocumentCommandFactory
	{
		public IDocumentCommand CreateCommand(IClosedDocument closedDocument) => DoNothingCommand.Instance;
	}
}