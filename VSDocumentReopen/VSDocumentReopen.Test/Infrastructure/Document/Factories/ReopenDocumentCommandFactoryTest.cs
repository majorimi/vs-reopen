using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;
using VSDocumentReopen.Infrastructure.Document.Factories;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Factories
{
	public class ReopenDocumentCommandFactoryTest
	{
		[Fact]
		public void ItShouldHandle_Nulls()
		{
			var factory = new ReopenDocumentCommandFactory(null);
			var command = factory.CreateCommand(null);

			Assert.NotNull(factory);
			Assert.NotNull(command);
		}

		[Fact]
		public void ItShouldCreate_ReopenDocumentCommand()
		{
			var factory = new ReopenDocumentCommandFactory(null);
			var command = factory.CreateCommand(NullDocument.Instance);

			Assert.IsType<ReopenDocumentCommand>(command);
		}
	}
}