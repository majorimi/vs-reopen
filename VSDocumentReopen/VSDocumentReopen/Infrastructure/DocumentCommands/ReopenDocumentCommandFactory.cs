using EnvDTE;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentCommands
{
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