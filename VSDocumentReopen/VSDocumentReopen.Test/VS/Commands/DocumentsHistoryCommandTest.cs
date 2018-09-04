using Microsoft.VisualStudio.Shell;
using Moq;
using System;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.Test.AssemblyFictures;
using VSDocumentReopen.VS.Commands;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class DocumentsHistoryCommandTest : VisualStudioCommandTestBase<DocumentsHistoryCommand>, IAssemblyFixture<ConfigContext>
	{
		private readonly Mock<IDocumentHistoryQueries> _documentHistoryQueriesMock;
		private readonly Mock<IHistoryCommand> _historyCommandMock;
		private readonly Mock<IHistoryCommandFactory> _historyCommandFactoryMock;

		private readonly DocumentsHistoryCommand _documentsHistoryCommand;
		private readonly ConfigContext _configContext;

		public DocumentsHistoryCommandTest(ConfigContext configContext)
		{
			_documentHistoryQueriesMock = new Mock<IDocumentHistoryQueries>();

			_historyCommandMock = new Mock<IHistoryCommand>();
			_historyCommandMock.Setup(s => s.Execute());

			_historyCommandFactoryMock = new Mock<IHistoryCommandFactory>();

			Task.Run(() => DocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object,
				_documentHistoryQueriesMock.Object,
				_historyCommandFactoryMock.Object)).Wait();
			_documentsHistoryCommand = DocumentsHistoryCommand.Instance;
			_configContext = configContext;
		}

		[Fact]
		public void CommandId_ShouldBe()
		{
			Assert.Equal(0x0200, DocumentsHistoryCommand.CommandId);
		}

		[Fact]
		public async Task ItShould_Handle_Null_AsyncPackageAsync()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => DocumentsHistoryCommand.InitializeAsync(null, _documentHistoryQueriesMock.Object,
					_historyCommandFactoryMock.Object));
			});
		}

		[Fact]
		public async void ItShould_Handle_Null_IDocumentHistoryQueries()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => DocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object, null, _historyCommandFactoryMock.Object));
			});
		}

		[Fact]
		public async void ItShould_Handle_Null_IHistoryCommandFactory()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => DocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object, _documentHistoryQueriesMock.Object, null));
			});
		}

		[Fact]
		public void Execute_Command_Should_Do_Nothing()
		{
			InvokeCommand(_documentsHistoryCommand);

			_documentHistoryQueriesMock.Verify(v => v.Get(It.IsAny<int>()), Times.Never);
			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.IsAny<IClosedDocument[]>()), Times.Never);
		}

		[Fact]
		public void ItShould_Handle_Empty_Document_Hisotry()
		{
			var cmd = new OleMenuCommand(null, null);
			_documentHistoryQueriesMock.Setup(s => s.Get(It.Is<int>(p => p == 5))).Returns(new IClosedDocument[0]);
			_configContext.Configuration.SetupGet(s => s.MaxNumberOfHistoryItemsOnMenu).Returns(5);

			InvokeCommand(_documentsHistoryCommand, "DynamicStartBeforeQueryStatus", cmd);

			_configContext.Configuration.VerifyGet(v => v.MaxNumberOfHistoryItemsOnMenu, Times.Once);
			_documentHistoryQueriesMock.Verify(v => v.Get(It.Is<int>(p => p == 5)), Times.Once);
			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.IsAny<IClosedDocument[]>()), Times.Never);
			Assert.True(cmd.Visible);
			Assert.Equal("<No History>", cmd.Text);
		}

		[Fact]
		public void ItShould_Show_Document_Hisotry()
		{
			var cmd = new OleMenuCommand(null, null);
			_documentHistoryQueriesMock.Setup(s => s.Get(It.Is<int>(p => p == 3))).Returns(new IClosedDocument[]
				{ NullDocument.Instance, NullDocument.Instance });
			_configContext.Configuration.SetupGet(s => s.MaxNumberOfHistoryItemsOnMenu).Returns(3);

			InvokeCommand(_documentsHistoryCommand, "DynamicStartBeforeQueryStatus", cmd);

			_configContext.Configuration.VerifyGet(v => v.MaxNumberOfHistoryItemsOnMenu, Times.AtLeastOnce);
			_documentHistoryQueriesMock.Verify(v => v.Get(It.Is<int>(p => p == 3)), Times.Once);
			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.IsAny<IClosedDocument[]>()), Times.Never);
			Assert.True(cmd.Visible);
			Assert.Equal("<History>", cmd.Text);
		}

		[Fact]
		public void ItShould_Create_And_Execute_History_Command_With_NullDocument()
		{
			var cmd = new OleMenuCommand(null, null);
			_historyCommandFactoryMock.Setup(s => s.CreateCommand(It.Is<IClosedDocument[]>(p => p[0] == NullDocument.Instance))).Returns(_historyCommandMock.Object);

			InvokeCommand(_documentsHistoryCommand, "DynamicCommandCallback", cmd);

			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.Is<IClosedDocument[]>(p => p[0] == NullDocument.Instance)), Times.Once);
			_historyCommandMock.Verify(v => v.Execute(), Times.Once);
		}

		[Fact]
		public void ItShould_Create_And_Execute_History_Command()
		{
			var doc = new ClosedDocument();
			var cmd = new OleMenuCommand(null, null);
			cmd.Properties.Add("HistoryItem", doc);
			_historyCommandFactoryMock.Setup(s => s.CreateCommand(It.Is<IClosedDocument[]>(p => p[0] == doc))).Returns(_historyCommandMock.Object);

			InvokeCommand(_documentsHistoryCommand, "DynamicCommandCallback", cmd);

			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.Is<IClosedDocument[]>(p => p[0] == doc)), Times.Once);
			_historyCommandMock.Verify(v => v.Execute(), Times.Once);
		}
	}
}