using VSDocumentReopen.Infrastructure.DocumentTracking;

namespace VSDocumentReopen.Infrastructure.HistoryCommands
{
	public class ClearHistoryCommand : IHistoryCommand
	{
		private readonly IDocumentHistoryCommands _documentHistoryCommands;

		public ClearHistoryCommand(IDocumentHistoryCommands documentHistoryCommands)
		{
			_documentHistoryCommands = documentHistoryCommands;
		}

		public void Execute()
		{
			_documentHistoryCommands.Clear();
		}
	}
}