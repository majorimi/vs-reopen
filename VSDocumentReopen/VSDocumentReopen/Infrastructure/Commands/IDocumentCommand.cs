using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Commands
{
	public interface IDocumentCommand
	{
		void Execute();
	}

	public class ReopenDocumentCommand : IDocumentCommand
	{
		private readonly _DTE _dte;
		private readonly IClosedDocument _closedDocument;

		public ReopenDocumentCommand(_DTE dte, IClosedDocument closedDocument)
		{
			_dte = dte;
			_closedDocument = closedDocument;
		}

		public void Execute()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			_dte.ItemOperations.OpenFile(_closedDocument.FullName, _closedDocument.Kind);
		}
	}

	//public class RemoveHistoryCommand : IDocumentCommand
	//{
	//	private readonly IClosedDocument[] _closedDocument;

	//	public RemoveHistoryCommand(params IClosedDocument[] closedDocuments)
	//	{
	//		_closedDocument = closedDocuments;
	//	}

	//	public void Execute()
	//	{
	//		var history = DocumentHistoryManager.Instance.GetAll();

	//		history.
	//	}
	//}

	//public class ExecuteCommandAndRemoveHistoryCommand : IDocumentCommand
	//{
	//	private readonly IDocumentCommandFactory _commandFactory;
	//	private readonly IClosedDocument[] _closedDocuments;
	//	private readonly IDocumentCommand _removeHistoryCommand;

	//	public ExecuteCommandAndRemoveHistoryCommand(IDocumentCommandFactory commandFactory, params IClosedDocument[] closedDocuments)
	//	{
	//		_commandFactory = commandFactory;
	//		_closedDocuments = closedDocuments;

	//		_removeHistoryCommand = new RemoveHistoryCommand(closedDocuments);
	//	}

	//	public void Execute()
	//	{
	//		foreach (var doc in _closedDocuments)
	//		{
	//			var cmd = _commandFactory.CreateCommand(doc);
	//			cmd.Execute();
	//		}

	//		_removeHistoryCommand.Execute();
	//	}
	//}
}