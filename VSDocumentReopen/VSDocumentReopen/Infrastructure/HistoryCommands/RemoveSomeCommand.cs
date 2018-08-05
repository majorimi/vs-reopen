using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;

namespace VSDocumentReopen.Infrastructure.HistoryCommands
{
	public class RemoveSomeCommand : IHistoryCommand
	{
		private readonly IDocumentHistoryCommands _documentHistoryCommands;
		private readonly IDocumentCommandFactory _documentCommandFactory;
		private readonly IClosedDocument[] _closedDocuments;

		public RemoveSomeCommand(IDocumentHistoryCommands documentHistoryCommands,
			IDocumentCommandFactory documentCommandFactory,
			params IClosedDocument[] closedDocuments)
		{
			_documentHistoryCommands = documentHistoryCommands;
			_documentCommandFactory = documentCommandFactory;
			_closedDocuments = closedDocuments;
		}

		public void Execute()
		{
			foreach (var doc in _closedDocuments)
			{
				var cmd = _documentCommandFactory?.CreateCommand(doc);
				cmd.Execute();
			}

			_documentHistoryCommands?.Remove(_closedDocuments);
		}
	}
}