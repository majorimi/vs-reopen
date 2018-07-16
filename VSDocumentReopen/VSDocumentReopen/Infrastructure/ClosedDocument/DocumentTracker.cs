using System;
using System.IO;
using EnvDTE;
using VSDocumentReopen.Domain;

namespace VSDocumentReopen.Infrastructure.ClosedDocument
{
	internal sealed class DocumentTracker
	{
		private readonly _DTE _dte;
		private readonly IHistoryRepositoryFactory _historyRepositoryFactory;
		private readonly SolutionEvents _solutionEvents;
		private readonly DocumentEvents _documentEvents;

		private SolutionInfo _currentSolution;

		public DocumentTracker(_DTE dte, IHistoryRepositoryFactory historyRepositoryFactory)
		{
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));
			_historyRepositoryFactory = historyRepositoryFactory;

			_solutionEvents = _dte.Events.SolutionEvents;
			_documentEvents = _dte.Events.DocumentEvents;

			Initialize();
		}

		private void Initialize()
		{
			_solutionEvents.Opened += () =>
			{
				_documentEvents.DocumentClosing += DocumentEventsOnDocumentClosing;

				var solutionDir = Path.GetDirectoryName(_dte.Solution.FullName);
				var solutionName = Path.GetFileName(_dte.Solution.FullName).Replace(".sln", string.Empty);
				_currentSolution = new SolutionInfo(solutionDir, solutionName);
				
				//Create history repo
				var historyRepository = _historyRepositoryFactory.CreateHistoryRepository(_currentSolution);
				
				//Load history and init state
				var history = historyRepository.GetHistory();
				DocumentHistory.Instance.Initialize(history);
			};
			_solutionEvents.BeforeClosing += () =>
			{
				//TODO: User can cancel the "Close" operation which means no history after that. However using the "AfterClosing" event will push all opened doc to the history...
				_documentEvents.DocumentClosing -= DocumentEventsOnDocumentClosing;

				//Save state
				var historyRepository = _historyRepositoryFactory.CreateHistoryRepository(_currentSolution);
				if (!historyRepository.SaveHistory(DocumentHistory.Instance.GetAll()))
				{
					//TODO: log and notify user...
				}

				_currentSolution = null;
				DocumentHistory.Instance.Clear();
			};
		}

		private void DocumentEventsOnDocumentClosing(Document document)
		{
			DocumentHistory.Instance.AddClosed(document);
		}
	}
}