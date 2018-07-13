using EnvDTE;
using EnvDTE80;
using System;

namespace VSDocumentReopen.Documents
{
	internal sealed class DocumentTracker
	{
		private readonly DTE2 _dte;
		private readonly SolutionEvents _solutionEvents;
		private readonly DocumentEvents _documentEvents;

		public DocumentTracker(DTE2 dte)
		{
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));

			_solutionEvents = _dte.Events.SolutionEvents;
			_documentEvents = _dte.Events.DocumentEvents;

			Initialize();
		}

		private void Initialize()
		{
			_solutionEvents.Opened += () =>
			{
				DocumentHistory.Instance.Clear();
				_documentEvents.DocumentClosing += DocumentEventsOnDocumentClosing;

				//TODO: Load state
				string solutionDir = System.IO.Path.GetDirectoryName(_dte.Solution.FullName);
				var tmpFile = System.IO.Path.Combine(solutionDir, ".vs", "VSDocumentReopen", "temp.db");
				System.IO.File.Create(tmpFile);
			};
			_solutionEvents.BeforeClosing += () =>
			{
				//TODO: User can cancel the "Close" operation which means no history after that. However using the "AfterClosing" event will push all opened doc to the history...
				_documentEvents.DocumentClosing -= DocumentEventsOnDocumentClosing;
				DocumentHistory.Instance.Clear();

				//TODO: Save state
			};
		}

		private void DocumentEventsOnDocumentClosing(Document document)
		{
			DocumentHistory.Instance.AddClosed(document);
		}
	}
}