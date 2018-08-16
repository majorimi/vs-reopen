using System;
using System.Reflection;
using System.Threading.Tasks;
using VSDocumentReopen.VS.Commands;
using Xunit;

namespace VSDocumentReopen.Test.VS.Commands
{
	public class ShowDocumentsHIstoryCommandTest : VisualStudioCommandTestBase<ShowDocumentsHIstoryCommand>
	{
		private readonly ShowDocumentsHIstoryCommand _showDocumentsHIstoryCommand;

		public ShowDocumentsHIstoryCommandTest()
		{
			Task.Run(() => ShowDocumentsHIstoryCommand.InitializeAsync(_asyncPackageMock.Object)).Wait();

			_showDocumentsHIstoryCommand = ShowDocumentsHIstoryCommand.Instance;
		}

		[Fact]
		public void CommandId_ShouldBe()
		{
			Assert.Equal(0x0110, ShowDocumentsHIstoryCommand.CommandId);
		}

		[Fact]
		public async Task ItShould_Handle_Null_AsyncPackageAsync()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
			{
				return Task.Run(() => ShowDocumentsHIstoryCommand.InitializeAsync(null));
			});
		}

		[Fact]
		public void ItShould_NotWork_Without_VS()
		{
			Assert.Throws<TargetInvocationException>(() => InvokeCommand(_showDocumentsHIstoryCommand));
		}
	}
}