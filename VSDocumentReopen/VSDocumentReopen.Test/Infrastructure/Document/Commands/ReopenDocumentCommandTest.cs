using EnvDTE;
using Moq;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Commands;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Commands
{
	public class ReopenDocumentCommandTest
	{
		private readonly Mock<_DTE> _dteMock;
		private readonly Mock<ItemOperations> _itemOperationsMock;
		private readonly Mock<IClosedDocument> _closedDecumentMock;

		public ReopenDocumentCommandTest()
		{
			_itemOperationsMock = new Mock<ItemOperations>();
			_itemOperationsMock.Setup(s => s.OpenFile(It.IsAny<string>(), It.IsAny<string>()));

			_dteMock = new Mock<_DTE>();
			_dteMock.SetupGet(g => g.ItemOperations).Returns(_itemOperationsMock.Object);

			_closedDecumentMock = new Mock<IClosedDocument>();
		}

		[Fact]
		public void ItShould_Handle_Null_DTE()
		{
			_closedDecumentMock.Setup(s => s.IsValid()).Returns(true);

			var command = new ReopenDocumentCommand(null, _closedDecumentMock.Object);

			command.Execute();

			Assert.NotNull(command);
			_itemOperationsMock.Verify(v => v.OpenFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

		}

		[Fact]
		public void ItShould_Handle_Invalid_Document()
		{
			_closedDecumentMock.Setup(s => s.IsValid()).Returns(false);

			var command = new ReopenDocumentCommand(_dteMock.Object, _closedDecumentMock.Object);

			command.Execute();

			Assert.NotNull(command);
			_itemOperationsMock.Verify(v => v.OpenFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[Fact]
		public void ItShould_Open_Valid_Document()
		{
			_closedDecumentMock.Setup(s => s.IsValid()).Returns(true);
			_closedDecumentMock.SetupGet(s => s.FullName).Returns("doc.cs");
			_closedDecumentMock.Setup(s => s.Kind).Returns("kind");

			var command = new ReopenDocumentCommand(_dteMock.Object, _closedDecumentMock.Object);

			command.Execute();

			Assert.NotNull(command);
			_itemOperationsMock.Verify(s => s.OpenFile(It.Is<string>(p => p == "doc.cs"), It.Is<string>(p => p == "kind")), Times.Once);
		}
	}
}