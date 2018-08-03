using EnvDTE;
using Moq;
using System;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Tracking
{
	public class DocumentEventsTrackerTest : IDisposable
	{
		private readonly Mock<Events> _eventsMock;
		private readonly Mock<SolutionEvents> _solutionEventsMock;
		private readonly Mock<DocumentEvents> _documentEventsMock;
		
		private readonly Mock<_DTE> _dteMock;
		private readonly Mock<IDocumentHistoryManager> _documentHistoryManagerMock;
		private readonly Mock<IHistoryRepositoryFactory> _historyRepositoryFactoryMock;

		private readonly DocumentEventsTracker _documentEventsTracker;

		public DocumentEventsTrackerTest()
		{
			_solutionEventsMock = new Mock<SolutionEvents>(MockBehavior.Strict);
			//_solutionEventsMock.As<_dispSolutionEvents_Event>().Setup(s => s.add_Opened(null));

			_documentEventsMock = new Mock<DocumentEvents>();

			_eventsMock = new Mock<Events>();
			_eventsMock.SetupGet(g => g.SolutionEvents).Returns(_solutionEventsMock.Object);
			_eventsMock.Setup(g => g.get_DocumentEvents(It.IsAny<EnvDTE.Document>())).Returns(_documentEventsMock.Object);

			_dteMock = new Mock<_DTE>();
			_dteMock.SetupGet(g => g.Events).Returns(_eventsMock.Object);

			_documentHistoryManagerMock = new Mock<IDocumentHistoryManager>();
			_historyRepositoryFactoryMock = new Mock<IHistoryRepositoryFactory>();

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object);
		}

		[Fact]
		public void ItShould_ThrowIf_DTE_Null()
		{
			Assert.Throws<ArgumentNullException>("dte", () => new DocumentEventsTracker(null, null, null));
		}

		public void Dispose()
		{
			_documentEventsTracker?.Dispose();
		}
	}
}