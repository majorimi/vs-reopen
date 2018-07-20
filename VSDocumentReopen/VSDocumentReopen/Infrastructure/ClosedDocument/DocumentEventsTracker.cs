using System;
using System.IO;
using EnvDTE;
using VSDocumentReopen.Domain;

namespace VSDocumentReopen.Infrastructure.ClosedDocument
{
	public enum SolutionStates
	{
		None = 0,
		Opened = 1,
		StartedToClose = 2
	}

	public sealed class DocumentEventsTracker : IDisposable
	{
		private readonly _DTE _dte;
		private readonly IHistoryRepositoryFactory _historyRepositoryFactory;
		private readonly SolutionEvents _solutionEvents;
		private readonly DocumentEvents _documentEvents;

		private SolutionInfo _currentSolution;

		public SolutionStates SolutionState { get; private set; }

		public DocumentEventsTracker(_DTE dte, IHistoryRepositoryFactory historyRepositoryFactory)
		{
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));
			_historyRepositoryFactory = historyRepositoryFactory;

			_solutionEvents = _dte.Events.SolutionEvents;
			_documentEvents = _dte.Events.DocumentEvents;

			Initialize();
		}

		private void Initialize()
		{
			SolutionState = SolutionStates.None;

			_solutionEvents.Opened += OnSolutionEventsOnOpened;
			_solutionEvents.BeforeClosing += OnSolutionEventsOnBeforeClosing;
			_solutionEvents.AfterClosing += OnSolutionEventsOnAfterClosing;
		}

		private void OnSolutionEventsOnOpened()
		{
			_documentEvents.DocumentClosing += DocumentEventsOnDocumentClosing;
			SolutionState = SolutionStates.Opened;

			var solutionDir = Path.GetDirectoryName(_dte.Solution.FullName);
			var solutionName = Path.GetFileName(_dte.Solution.FullName).Replace(".sln", string.Empty);
			_currentSolution = new SolutionInfo(solutionDir, solutionName);

			//Create history repo
			var historyRepository = _historyRepositoryFactory.CreateHistoryRepository(_currentSolution);

			//Load history and init state
			var history = historyRepository.GetHistory();
			DocumentHistoryManager.Instance.Initialize(history);
		}

		private void OnSolutionEventsOnBeforeClosing()
		{
			//TODO: try to validate if it was closed or canceled...
			SolutionState = SolutionStates.StartedToClose;
		}

		private void OnSolutionEventsOnAfterClosing()
		{
			_documentEvents.DocumentClosing -= DocumentEventsOnDocumentClosing;

			//Save state
			var historyRepository = _historyRepositoryFactory.CreateHistoryRepository(_currentSolution);
			if (!historyRepository.SaveHistory(DocumentHistoryManager.Instance.GetAll()))
			{
				//TODO: log and notify user...
			}

			SolutionState = SolutionStates.None;
			_currentSolution = null;
			DocumentHistoryManager.Instance.Clear();
		}

		private void DocumentEventsOnDocumentClosing(Document document)
		{
			if (SolutionState == SolutionStates.Opened)
			{
				DocumentHistoryManager.Instance.AddClosed(document);
			}
		}

		public void Dispose()
		{
			_solutionEvents.Opened -= OnSolutionEventsOnOpened;
			_solutionEvents.BeforeClosing -= OnSolutionEventsOnBeforeClosing;
			_solutionEvents.AfterClosing -= OnSolutionEventsOnAfterClosing;

			_documentEvents.DocumentClosing -= DocumentEventsOnDocumentClosing;
		}
	}
}