using Moq;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;
using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.HistoryCommands
{
	public class RemoveSomeCommandTest
	{
		private readonly RemoveSomeCommand _removeSomeCommand;
		private readonly Mock<IDocumentHistoryCommands> _documentHistoryCommandsMock;
		private readonly Mock<IDocumentCommandFactory> _documentCommandFactoryMock;
		private readonly Mock<IDocumentCommand> _documentCommandMock;

		public RemoveSomeCommandTest()
		{
			_documentHistoryCommandsMock = new Mock<IDocumentHistoryCommands>();
			_documentHistoryCommandsMock.Setup(s => s.RemoveLast()).Returns(NullDocument.Instance);

			_documentCommandMock = new Mock<IDocumentCommand>();

			_documentCommandFactoryMock = new Mock<IDocumentCommandFactory>();
			_documentCommandFactoryMock.Setup(s => s.CreateCommand(It.IsAny<IClosedDocument>()))
				.Returns(_documentCommandMock.Object);

			_removeSomeCommand = new RemoveSomeCommand(_documentHistoryCommandsMock.Object, _documentCommandFactoryMock.Object,
				NullDocument.Instance, NullDocument.Instance);
		}

		[Fact]
		public void ItShould_Handle_Nulls()
		{
			var command = new RemoveSomeCommand(null, null);

			command.Execute();

			_documentHistoryCommandsMock.Verify(v => v.RemoveLast(), Times.Never);
			_documentCommandFactoryMock.Verify(v => v.CreateCommand(It.IsAny<IClosedDocument>()), Times.Never);
			_documentCommandMock.Verify(v => v.Execute(), Times.Never);
		}

		[Fact]
		public void ItShould_Remove_Last_Document()
		{
			_removeSomeCommand.Execute();

			_documentCommandFactoryMock.Verify(v => v.CreateCommand(It.Is<IClosedDocument>(p => p == NullDocument.Instance)), Times.Exactly(2));
			_documentCommandMock.Verify(v => v.Execute(), Times.Exactly(2));
			_documentHistoryCommandsMock.Verify(v => v.Remove(It.IsAny<IClosedDocument[]>()), Times.Once);
		}
	}
}