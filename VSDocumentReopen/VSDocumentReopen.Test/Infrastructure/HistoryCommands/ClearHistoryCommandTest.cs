using Moq;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.HistoryCommands
{
	public class ClearHistoryCommandTest
	{
		private readonly Mock<IDocumentHistoryCommands> _documentHistoryCommandMock;

		public ClearHistoryCommandTest()
		{
			_documentHistoryCommandMock = new Mock<IDocumentHistoryCommands>();
			_documentHistoryCommandMock.Setup(s => s.Clear());
		}

		[Fact]
		public void ItShould_Handle_Null()
		{
			var command = new ClearHistoryCommand(null);

			command.Execute();

			_documentHistoryCommandMock.Verify(v => v.Clear(), Times.Never);
		}

		[Fact]
		public void ItShould_Call_Clear()
		{
			var command = new ClearHistoryCommand(_documentHistoryCommandMock.Object);

			command.Execute();

			_documentHistoryCommandMock.Verify(v => v.Clear(), Times.Once);
		}
	}
}