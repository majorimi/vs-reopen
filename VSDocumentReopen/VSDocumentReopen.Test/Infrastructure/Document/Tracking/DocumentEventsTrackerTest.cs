using EnvDTE;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.Repositories;
using VSDocumentReopen.VS.MessageBox;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Tracking
{
	public class DocumentEventsTrackerTest : IDisposable
	{
		private readonly Mock<Solution> _solutionMock;
		private readonly Mock<Events> _eventsMock;
		private readonly SolutionEvents _solutionEventsMock;
		private readonly DocumentEvents _documentEventsMock;
		
		private readonly Mock<_DTE> _dteMock;
		private readonly Mock<IDocumentHistoryManager> _documentHistoryManagerMock;
		private readonly Mock<IHistoryRepositoryFactory> _historyRepositoryFactoryMock;
		private readonly Mock<IHistoryRepository> _historyRepositoryMock;
		private readonly Mock<IMessageBox> _messageBoxMock;

		private DocumentEventsTracker _documentEventsTracker;

		public DocumentEventsTrackerTest()
		{
			_solutionEventsMock = Substitute.For<SolutionEvents>();
			_solutionEventsMock.Opened += Arg.Any<_dispSolutionEvents_OpenedEventHandler>();
			_solutionEventsMock.BeforeClosing += Arg.Any<_dispSolutionEvents_BeforeClosingEventHandler>();
			_solutionEventsMock.AfterClosing += Arg.Any<_dispSolutionEvents_AfterClosingEventHandler>();

			_documentEventsMock = Substitute.For<DocumentEvents>();
			_documentEventsMock.DocumentClosing += Arg.Any<_dispDocumentEvents_DocumentClosingEventHandler>();

			_eventsMock = new Mock<Events>();
			_eventsMock.SetupGet(g => g.SolutionEvents).Returns(_solutionEventsMock);
			_eventsMock.Setup(g => g.get_DocumentEvents(It.IsAny<EnvDTE.Document>())).Returns(_documentEventsMock);

			_solutionMock = new Mock<Solution>();
			_solutionMock.SetupGet(g => g.FullName).Returns("c:\\test.sln");
			_solutionMock.SetupGet(g => g.IsOpen).Returns(false);

			_dteMock = new Mock<_DTE>();
			_dteMock.SetupGet(g => g.Events).Returns(_eventsMock.Object);
			_dteMock.SetupGet(g => g.Solution).Returns(_solutionMock.Object);

			_documentHistoryManagerMock = new Mock<IDocumentHistoryManager>();
			_documentHistoryManagerMock.Setup(s => s.Initialize(It.IsAny<IEnumerable<IClosedDocument>>()));
			_historyRepositoryFactoryMock = new Mock<IHistoryRepositoryFactory>();

			_historyRepositoryMock = new Mock<IHistoryRepository>();

			_messageBoxMock = new Mock<IMessageBox>();
			_messageBoxMock.Setup(s => s.ShowError(It.IsAny<string>(), It.IsAny<string>()));

		}

		[Fact]
		public void ItShould_Throw_Whne_Constructor_Params_Null()
		{
			Assert.Throws<ArgumentNullException>("dte", () => new DocumentEventsTracker(null, null, null, null));
			Assert.Throws<ArgumentNullException>("documentHistoryManager", () => new DocumentEventsTracker(_dteMock.Object, null, null, null));
			Assert.Throws<ArgumentNullException>("historyRepositoryFactory", () => new DocumentEventsTracker(_dteMock.Object, _documentHistoryManagerMock.Object, null, null));
			Assert.Throws<ArgumentNullException>("messageBox", () => new DocumentEventsTracker(_dteMock.Object, _documentHistoryManagerMock.Object, _historyRepositoryFactoryMock.Object, null));
		}

		[Fact]
		public void ItShould_Initialize_Object()
		{
			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_solutionEventsMock.Received(1).Opened += Arg.Any<_dispSolutionEvents_OpenedEventHandler>();
			_solutionEventsMock.Received(1).BeforeClosing += Arg.Any<_dispSolutionEvents_BeforeClosingEventHandler>();
			_solutionEventsMock.Received(1).AfterClosing += Arg.Any<_dispSolutionEvents_AfterClosingEventHandler>();

			Assert.Equal(SolutionStates.None, _documentEventsTracker.SolutionState);
		}

		[Fact]
		public void ItShould_Handle_SolutionOpened_Event_With_RepositoryError()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.IsAny<SolutionInfo>()))
				.Returns(() => null);

			_solutionEventsMock.Opened += Raise.Event<_dispSolutionEvents_OpenedEventHandler>();

			_documentEventsMock.Received(1).DocumentClosing += Arg.Any<_dispDocumentEvents_DocumentClosingEventHandler>();
			Assert.Equal(SolutionStates.Opened, _documentEventsTracker.SolutionState);
			_solutionMock.VerifyGet(v => v.FullName, Times.Exactly(2));
			_historyRepositoryFactoryMock.Verify(v => v.CreateHistoryRepository(It.IsAny<SolutionInfo>()), Times.Once);
			_historyRepositoryMock.Verify(v => v.GetHistory(), Times.Never);
			_documentHistoryManagerMock.Verify(v => v.Initialize(It.Is<IEnumerable<IClosedDocument>>(l => l == null)), Times.Once);
		}

		[Fact]
		public void ItShould_Handle_SolutionOpened_Event()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.Is<SolutionInfo>(si => si.FullPath == "c:\\" && si.Name == "test")))
				.Returns(_historyRepositoryMock.Object);
			_historyRepositoryMock.Setup(s => s.GetHistory())
				.Returns(new List<IClosedDocument>());

			_solutionEventsMock.Opened += Raise.Event<_dispSolutionEvents_OpenedEventHandler>();

			_documentEventsMock.Received(1).DocumentClosing += Arg.Any<_dispDocumentEvents_DocumentClosingEventHandler>();
			Assert.Equal(SolutionStates.Opened, _documentEventsTracker.SolutionState);
			_solutionMock.VerifyGet(v => v.FullName, Times.Exactly(2));
			_historyRepositoryFactoryMock.Verify(v => v.CreateHistoryRepository(
				It.Is<SolutionInfo>(si => si.FullPath == "c:\\" && si.Name == "test")),
				Times.Once);
			_historyRepositoryMock.Verify(v => v.GetHistory(), Times.Once);
			_documentHistoryManagerMock.Verify(v => v.Initialize(It.Is<List<IClosedDocument>>(l => l.Count == 0)), Times.Once);
		}

		[Fact]
		public void ItShould_Handle_SolutionBeforeClosing_Event()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.IsAny<SolutionInfo>()))
				.Returns(_historyRepositoryMock.Object);

			_solutionEventsMock.BeforeClosing += Raise.Event<_dispSolutionEvents_BeforeClosingEventHandler>();

			Assert.Equal(SolutionStates.StartedToClose, _documentEventsTracker.SolutionState);
		}

		[Fact]
		public void ItShould_Handle_SolutionAfterClosing_Event()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.IsAny<SolutionInfo>()))
				.Returns(_historyRepositoryMock.Object);
			_historyRepositoryMock.Setup(s => s.SaveHistory(It.IsAny<IEnumerable<IClosedDocument>>())).Returns(true);
			_documentHistoryManagerMock.Setup(s => s.GetAll())
				.Returns(new List<IClosedDocument>());

			_solutionEventsMock.AfterClosing += Raise.Event<_dispSolutionEvents_AfterClosingEventHandler>();

			_documentEventsMock.Received(1).DocumentClosing -= Arg.Any<_dispDocumentEvents_DocumentClosingEventHandler>();
			_historyRepositoryFactoryMock.Verify(v => v.CreateHistoryRepository(It.IsAny<SolutionInfo>()), Times.Once);
			_historyRepositoryMock.Verify(v => v.SaveHistory(It.IsAny<IEnumerable<IClosedDocument>>()), Times.Once);
			_documentHistoryManagerMock.Verify(v => v.GetAll(), Times.Once);
			_documentHistoryManagerMock.Verify(v => v.Clear(), Times.Once);
			Assert.Equal(SolutionStates.None, _documentEventsTracker.SolutionState);
		}

		[Fact]
		public void ItShould_Notify_User_SolutionAfterClosing_Event_When_History_Was_Not_Saved()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.IsAny<SolutionInfo>()))
				.Returns(_historyRepositoryMock.Object);
			_historyRepositoryMock.Setup(s => s.SaveHistory(It.IsAny<IEnumerable<IClosedDocument>>())).Returns(false);
			_documentHistoryManagerMock.Setup(s => s.GetAll())
				.Returns(new List<IClosedDocument>());

			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_solutionEventsMock.AfterClosing += Raise.Event<_dispSolutionEvents_AfterClosingEventHandler>();

			_historyRepositoryMock.Verify(v => v.SaveHistory(It.IsAny<IEnumerable<IClosedDocument>>()), Times.Once);
			_messageBoxMock.Verify(v => v.ShowError(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[Fact]
		public void ItShould_Not_Handle_DocumentClosed_Event_Before_Solution_Loaded()
		{
			var doc = Substitute.For<EnvDTE.Document>();
			_documentEventsMock.DocumentClosing += Raise.Event<_dispDocumentEvents_DocumentClosingEventHandler>(doc);

			Assert.Equal(SolutionStates.None, _documentEventsTracker.SolutionState);
			_documentEventsMock.Received(1).DocumentClosing += Raise.Event<_dispDocumentEvents_DocumentClosingEventHandler>(doc);
			_documentHistoryManagerMock.Verify(v => v.Add(It.IsAny<IClosedDocument>()), Times.Never);
		}

		[Fact]
		public void ItShould_Handle_DocumentClosed_Event_When_Solution_Loaded()
		{
			_solutionMock.SetupGet(g => g.IsOpen).Returns(true);

			_documentEventsTracker = new DocumentEventsTracker(_dteMock.Object,
				_documentHistoryManagerMock.Object,
				_historyRepositoryFactoryMock.Object,
				_messageBoxMock.Object);

			var doc = Substitute.For<EnvDTE.Document>();
			doc.FullName.Returns("c:\\test.cs");
			doc.Name.Returns("test.cs");
			doc.Kind.Returns("kind");
			doc.Language.Returns("cs");
			var closedAt = DateTime.Now;

			_documentEventsMock.DocumentClosing += Raise.Event<_dispDocumentEvents_DocumentClosingEventHandler>(doc);
			_historyRepositoryFactoryMock.Setup(s => s.CreateHistoryRepository(It.IsAny<SolutionInfo>()))
				.Returns(() => null);
			_documentHistoryManagerMock.Setup(s => s.Add(It.IsAny<IClosedDocument>()));

			_solutionEventsMock.Opened += Raise.Event<_dispSolutionEvents_OpenedEventHandler>();

			Assert.Equal(SolutionStates.Opened, _documentEventsTracker.SolutionState);
			_documentEventsMock.Received(1).DocumentClosing += Raise.Event<_dispDocumentEvents_DocumentClosingEventHandler>(doc);
			_documentHistoryManagerMock.Verify(v => v.Add(It.Is<IClosedDocument>(p =>
				p.FullName == doc.FullName && p.Name == doc.Name && p.Kind == doc.Kind && p.Language == doc.Language
				&& p.ClosedAt >= closedAt)), Times.Once);
		}

		[Fact]
		public void ItShould_Unsubscribe_Events_When_Disposing()
		{
			_documentEventsTracker?.Dispose();

			_solutionEventsMock.Received(1).Opened -= Arg.Any<_dispSolutionEvents_OpenedEventHandler>();
			_solutionEventsMock.Received(1).BeforeClosing -= Arg.Any<_dispSolutionEvents_BeforeClosingEventHandler>();
			_solutionEventsMock.Received(1).AfterClosing -= Arg.Any<_dispSolutionEvents_AfterClosingEventHandler>();

			_documentEventsMock.Received(1).DocumentClosing -= Arg.Any<_dispDocumentEvents_DocumentClosingEventHandler>();
		}

		public void Dispose()
		{
			_documentEventsTracker?.Dispose();
		}
	}
}