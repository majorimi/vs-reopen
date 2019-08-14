using System;
using System.IO;
using EnvDTE;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Logging;
using VSDocumentReopen.VS.MessageBox;

namespace VSDocumentReopen.Infrastructure.Document.Tracking
{
	public sealed class DocumentEventsTracker : IDisposable
	{
		private readonly _DTE _dte;
		private readonly IDocumentHistoryManager _documentHistoryManager;
		private readonly IHistoryRepositoryFactory _historyRepositoryFactory;
		private readonly IMessageBox _messageBox;
		private readonly SolutionEvents _solutionEvents;
		private readonly DocumentEvents _documentEvents;

		private SolutionInfo _currentSolution;

		public SolutionStates SolutionState { get; private set; }

		public DocumentEventsTracker(_DTE dte,
			IDocumentHistoryManager documentHistoryManager,
			IHistoryRepositoryFactory historyRepositoryFactory,
			IMessageBox messageBox)
		{
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));
			_solutionEvents = _dte.Events.SolutionEvents;
			_documentEvents = _dte.Events.DocumentEvents;

			_documentHistoryManager = documentHistoryManager ?? throw new ArgumentNullException(nameof(documentHistoryManager));
			_historyRepositoryFactory = historyRepositoryFactory ?? throw new ArgumentNullException(nameof(historyRepositoryFactory));
			_messageBox = messageBox ?? throw new ArgumentNullException(nameof(messageBox));

			Initialize();
		}

		private void Initialize()
		{
            SolutionState = SolutionStates.None;

            _solutionEvents.Opened += OnSolutionEventsOnOpened;
			_solutionEvents.BeforeClosing += OnSolutionEventsOnBeforeClosing;
			_solutionEvents.AfterClosing += OnSolutionEventsOnAfterClosing;

            if(_dte.Solution.IsOpen) //VS2019 pushing for Async loading it is possible to have a loaded solution at this point...
            {
                OnSolutionEventsOnOpened();
            }
        }

        private void OnSolutionEventsOnOpened()
		{
			_documentEvents.DocumentClosing += DocumentEventsOnDocumentClosing;
			SolutionState = SolutionStates.Opened;

            LoggerContext.Current.Logger.Info($"{nameof(DocumentEventsTracker)}.{nameof(OnSolutionEventsOnOpened)}() method executing. Opened solution Full path: '{_dte.Solution.FullName}'");

            var solutionDir = Path.GetDirectoryName(_dte.Solution.FullName);
			var solutionName = Path.GetFileName(_dte.Solution.FullName).Replace(".sln", string.Empty);
			_currentSolution = new SolutionInfo(solutionDir, solutionName);

			//Create history repo
			var historyRepository = _historyRepositoryFactory.CreateHistoryRepository(_currentSolution);

			//Load history and init state
			var history = historyRepository?.GetHistory();
			_documentHistoryManager.Initialize(history);
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
			if (!historyRepository.SaveHistory(_documentHistoryManager.GetAll()))
			{
				_messageBox.ShowError("Visual Studio Document Reopen", "Failed to save Closed Document History!");
			}

			SolutionState = SolutionStates.None;
			_currentSolution = null;
			_documentHistoryManager.Clear();
		}

		private void DocumentEventsOnDocumentClosing(EnvDTE.Document document)
		{
			if (SolutionState == SolutionStates.Opened)
			{
				var doc = new ClosedDocument()
				{
					FullName = document.FullName,
					Name = document.Name,
					Kind = document.Kind,
					Language = document.Language,
					ClosedAt = DateTime.Now,
				};

				_documentHistoryManager.Add(doc);
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