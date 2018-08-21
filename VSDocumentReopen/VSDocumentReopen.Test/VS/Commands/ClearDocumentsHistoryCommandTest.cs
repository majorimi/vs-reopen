using System;
using Moq;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.Commands;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class ClearDocumentsHistoryCommandTest : VisualStudioCommandTestBase<ClearDocumentsHistoryCommand>
	{
		private readonly Mock<IHistoryCommand> _historyCommandMock;

		private readonly ClearDocumentsHistoryCommand _clearDocumentsHistoryCommand;

		public ClearDocumentsHistoryCommandTest()
		{
			_historyCommandMock = new Mock<IHistoryCommand>();
			_historyCommandMock.Setup(s => s.Execute());

			Task.Run(() => ClearDocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object, _historyCommandMock.Object)).Wait();
			_clearDocumentsHistoryCommand = ClearDocumentsHistoryCommand.Instance;
		}

		[Fact]
		public void CommandId_ShouldBe()
		{
			Assert.Equal(0x0104, ClearDocumentsHistoryCommand.CommandId);
		}

		[Fact]
		public async Task ItShould_Handle_Null_AsyncPackageAsync()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => ClearDocumentsHistoryCommand.InitializeAsync(null, _historyCommandMock.Object));
			});
		}

		[Fact]
		public async void ItShould_Handle_Null_IHistoryCommand()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => ClearDocumentsHistoryCommand.InitializeAsync(_asyncPackageMock.Object, null));
			});
		}

		[Fact]
		public void ItShould_Execute_Command()
		{
			InvokeCommand(_clearDocumentsHistoryCommand);

			_historyCommandMock.Verify(v => v.Execute(), Times.Once);
		}
	}
}