using Moq;
using System;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.Commands;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class DocumentsHistoryCommandTest : VisualStudioCommandTestBase<DocumentsHistoryCommand>
	{
		private readonly Mock<IDocumentHistoryQueries> _documentHistoryQueriesMock;
		private readonly Mock<IHistoryCommandFactory> _historyCommandFactoryMock;

		private readonly DocumentsHistoryCommand _documentsHistoryCommand;

		public DocumentsHistoryCommandTest()
		{
			_documentHistoryQueriesMock = new Mock<IDocumentHistoryQueries>();

			_historyCommandFactoryMock = new Mock<IHistoryCommandFactory>();

			Task.Run(() => DocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object,
				_documentHistoryQueriesMock.Object,
				_historyCommandFactoryMock.Object)).Wait();
			_documentsHistoryCommand = DocumentsHistoryCommand.Instance;
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
			InvoceCommand(_documentsHistoryCommand);

			_documentHistoryQueriesMock.Verify(v => v.Get(It.IsAny<int>()), Times.Never);
			_historyCommandFactoryMock.Verify(v => v.CreateCommand(It.IsAny<IClosedDocument[]>()), Times.Never);
		}
	}
}