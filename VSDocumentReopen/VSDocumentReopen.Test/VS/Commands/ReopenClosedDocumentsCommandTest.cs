using System;
using Moq;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.Commands;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class ReopenClosedDocumentsCommandTest : VisualStudioCommandTestBase<ReopenClosedDocumentsCommand>
	{
		private readonly Mock<IHistoryCommand> _historyCommandMock;

		private readonly ReopenClosedDocumentsCommand _reopenClosedDocumentsCommand;

		public ReopenClosedDocumentsCommandTest()
		{
			_historyCommandMock = new Mock<IHistoryCommand>();
			_historyCommandMock.Setup(s => s.Execute());

			Task.Run(() => ReopenClosedDocumentsCommand.InitializeAsync(_asyncPackageMock.Object,
				_historyCommandMock.Object)).Wait();

			_reopenClosedDocumentsCommand = ReopenClosedDocumentsCommand.Instance;
		}

		[Fact]
		public void CommandId_ShouldBe()
		{
			Assert.Equal(0x0101, ReopenClosedDocumentsCommand.CommandId);
		}

		[Fact]
		public async Task ItShould_Handle_Null_AsyncPackageAsync()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => ReopenClosedDocumentsCommand.InitializeAsync(null, _historyCommandMock.Object));
			});
		}

		[Fact]
		public async void ItShould_Handle_Null_IDocumentHistoryQueries()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => ReopenClosedDocumentsCommand.InitializeAsync(_asyncPackageMock.Object, null));
			});
		}

		[Fact]
		public void ItShould_Execute_Command()
		{
			InvokeCommand(_reopenClosedDocumentsCommand);

			_historyCommandMock.Verify(v => v.Execute(), Times.Once);
		}
	}
}