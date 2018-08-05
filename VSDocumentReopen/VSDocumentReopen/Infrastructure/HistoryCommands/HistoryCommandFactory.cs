using System;
using System.Linq;
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
			var constructors = typeParameterType.GetConstructors();

			var expectParams = constructors.Any(x => x.GetParameters().Length == 3
				&& x.GetParameters()[0].ParameterType == typeof(IDocumentHistoryCommands)
				&& x.GetParameters()[1].ParameterType == typeof(IDocumentCommandFactory)
				&& x.GetParameters()[2].ParameterType == typeof(IClosedDocument[]));

			if (expectParams)
			{
				return (IHistoryCommand)Activator.CreateInstance(typeParameterType,
					_documentHistoryCommands, _documentCommandFactory, closedDocuments);
			}

			return (IHistoryCommand)Activator.CreateInstance(typeParameterType);
		}
	}
}