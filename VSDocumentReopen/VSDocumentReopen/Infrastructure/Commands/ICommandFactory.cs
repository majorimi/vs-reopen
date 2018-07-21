using EnvDTE;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Commands
{
	public interface IDocumentCommandFactory
	{
		IDocumentCommand CreateCommand(IClosedDocument closedDocument);
	}

	//public interface IHistoryCommandFactory
	//{
	//	IDocumentCommand CreateCommand(params IClosedDocument[] closedDocument);
	//}

	public class ReopenDocumentCommandFactory : IDocumentCommandFactory
	{
		private readonly _DTE _dte;

		public ReopenDocumentCommandFactory(_DTE dte)
		{
			_dte = dte;
		}

		public IDocumentCommand CreateCommand(IClosedDocument closedDocument)
		{
			return new ReopenDocumentCommand(_dte, closedDocument);
		}
	}
}