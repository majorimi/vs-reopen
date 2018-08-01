using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;

namespace VSDocumentReopen.Infrastructure.HistoryCommands
{
	public class RemoveLastCommand : IHistoryCommand
	{
		private readonly IDocumentHistoryCommands _documentHistoryCommands;
		private readonly IDocumentCommandFactory _documentCommandFactory;

		public RemoveLastCommand(IDocumentHistoryCommands documentHistoryCommands,
			IDocumentCommandFactory documentCommandFactory)
		{
			_documentHistoryCommands = documentHistoryCommands;
			_documentCommandFactory = documentCommandFactory;
		}

		public void Execute()
		{
			var document = _documentHistoryCommands.RemoveLast();

			var command = _documentCommandFactory.CreateCommand(document);
			command.Execute();
		}
	}
}