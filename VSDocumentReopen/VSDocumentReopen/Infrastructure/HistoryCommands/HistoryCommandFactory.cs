using System;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;

namespace VSDocumentReopen.Infrastructure.HistoryCommands
{
	public class HistoryCommandFactory<T> : IHistoryCommandFactory
		where T : IHistoryCommand
	{
		private readonly IDocumentHistoryCommands _documentHistoryCommands;
		private readonly IDocumentCommandFactory _documentCommandFactory;

		public HistoryCommandFactory(IDocumentHistoryCommands documentHistoryCommands,
			IDocumentCommandFactory documentCommandFactory)
		{
			_documentHistoryCommands = documentHistoryCommands;
			_documentCommandFactory = documentCommandFactory;
		}

		public IHistoryCommand CreateCommand(params IClosedDocument[] closedDocuments)
		{
			Type typeParameterType = typeof(T);
			return (IHistoryCommand)Activator.CreateInstance(typeParameterType,
				_documentHistoryCommands, _documentCommandFactory, closedDocuments);
		}
	}
}