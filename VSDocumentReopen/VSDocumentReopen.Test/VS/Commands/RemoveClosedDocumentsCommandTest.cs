using System;
using Moq;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.Commands;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class RemoveClosedDocumentsCommandTest : VisualStudioCommandTestBase<RemoveClosedDocumentsCommand>
	{
		private readonly Mock<IHistoryCommand> _historyCommandMock;

		private readonly RemoveClosedDocumentsCommand _removeClosedDocumentsCommand;

		public RemoveClosedDocumentsCommandTest()
		{
			_historyCommandMock = new Mock<IHistoryCommand>();
			_historyCommandMock.Setup(s => s.Execute());

			Task.Run(() => RemoveClosedDocumentsCommand.InitializeAsync(_asyncPackageMock.Object,
				_historyCommandMock.Object)).Wait();

			_removeClosedDocumentsCommand = RemoveClosedDocumentsCommand.Instance;
		}

		[Fact]
		public void CommandId_ShouldBe()
		{
			Assert.Equal(0x0102, RemoveClosedDocumentsCommand.CommandId);
		}

		[Fact]
		public async Task ItShould_Handle_Null_AsyncPackageAsync()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => RemoveClosedDocumentsCommand.InitializeAsync(null, _historyCommandMock.Object));
			});
		}

		[Fact]
		public async void ItShould_Handle_Null_IDocumentHistoryQueries()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => RemoveClosedDocumentsCommand.InitializeAsync(_asyncPackageMock.Object, null));
			});
		}

		[Fact]
		public void ItShould_Execute_Command()
		{
			InvokeCommand(_removeClosedDocumentsCommand);

			_historyCommandMock.Verify(v => v.Execute(), Times.Once);
		}
	}
}