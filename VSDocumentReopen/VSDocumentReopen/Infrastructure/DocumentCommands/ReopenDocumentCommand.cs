using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.DocumentCommands
{
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

			if (_closedDocument.IsValid())
			{
				_dte.ItemOperations.OpenFile(_closedDocument.FullName, _closedDocument.Kind);
			}
		}
	}
}