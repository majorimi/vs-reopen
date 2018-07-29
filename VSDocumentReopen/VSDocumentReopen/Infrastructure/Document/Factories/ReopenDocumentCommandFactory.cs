using EnvDTE;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;

namespace VSDocumentReopen.Infrastructure.Document.Factories
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