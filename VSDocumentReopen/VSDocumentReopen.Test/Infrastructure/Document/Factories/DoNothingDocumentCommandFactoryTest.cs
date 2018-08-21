using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;
using VSDocumentReopen.Infrastructure.Document.Factories;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Factories
{
	public class DoNothingDocumentCommandFactoryTest
	{
		[Fact]
		public void ItShouldHandle_Null()
		{
			var factory = new DoNothingDocumentCommandFactory();
			var command = factory.CreateCommand(null);

			Assert.NotNull(factory);
			Assert.NotNull(command);
		}

		[Fact]
		public void ItShouldCreate_DoNothingCommand()
		{
			var factory = new DoNothingDocumentCommandFactory();
			var command = factory.CreateCommand(NullDocument.Instance);

			Assert.IsType<DoNothingCommand>(command);
		}

		[Fact]
		public void ItShouldReturn_Singleton()
		{
			var factory = new DoNothingDocumentCommandFactory();
			var command = factory.CreateCommand(NullDocument.Instance);
			var command2 = factory.CreateCommand(null);

			Assert.Equal(command, command2);
		}
	}
}