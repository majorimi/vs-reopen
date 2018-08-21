using VSDocumentReopen.Infrastructure.Document.Commands;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Commands
{
	public class DoNothingCommandTest
	{
		[Fact]
		public void ItShouldDo_Nothing()
		{
			var doNothing = DoNothingCommand.Instance;

			doNothing.Execute();
			Assert.NotNull(doNothing);
		}

		[Fact]
		public void ItShouldBe_Singleton()
		{
			var doNothing = DoNothingCommand.Instance;
			var doNothing2 = DoNothingCommand.Instance;

			Assert.Equal(doNothing, doNothing2);
		}
	}
}