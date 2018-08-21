using Moq;
using System;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.HistoryCommands
{
	public class HistoryCommandFactoryTest
	{
		private class FakeCommand : IHistoryCommand
		{
			public void Execute() => throw new NotImplementedException();
		}

		private class FakeCommand2 : IHistoryCommand
		{
			public IDocumentHistoryCommands DocumentHistoryCommands { get; }
			public IDocumentCommandFactory DocumentCommandFactory { get; }
			public IClosedDocument[] ClosedDocuments { get; }

			public FakeCommand2(IDocumentHistoryCommands documentHistoryCommands,
					IDocumentCommandFactory documentCommandFactory,
					params IClosedDocument[] closedDocuments)
			{
				DocumentHistoryCommands = documentHistoryCommands;
				DocumentCommandFactory = documentCommandFactory;
				ClosedDocuments = closedDocuments;
			}


			public void Execute() => throw new NotImplementedException();
		}

		private readonly Mock<IDocumentHistoryCommands> _documentHistoryCommandsMock;
		private readonly Mock<IDocumentCommandFactory> _documentCommandFactoryMock;

		public HistoryCommandFactoryTest()
		{
			_documentHistoryCommandsMock = new Mock<IDocumentHistoryCommands>();
			_documentCommandFactoryMock = new Mock<IDocumentCommandFactory>();
		}

		[Fact]
		public void ItShould_Handle_Null()
		{
			var factory = new HistoryCommandFactory<FakeCommand>(null, null);

			var command = factory.CreateCommand();

			Assert.NotNull(command);
			Assert.IsType<FakeCommand>(command);
		}

		[Fact]
		public void ItShould_Create_Command()
		{
			var factory = new HistoryCommandFactory<FakeCommand2>(_documentHistoryCommandsMock.Object, _documentCommandFactoryMock.Object);

			var command = factory.CreateCommand();

			Assert.NotNull(command);
			Assert.IsType<FakeCommand2>(command);
			Assert.Equal((command as FakeCommand2).DocumentHistoryCommands, _documentHistoryCommandsMock.Object);
			Assert.Equal((command as FakeCommand2).DocumentCommandFactory, _documentCommandFactoryMock.Object);
		}

		[Fact]
		public void ItShould_Create_Command_With_Documents()
		{
			var factory = new HistoryCommandFactory<FakeCommand2>(_documentHistoryCommandsMock.Object, _documentCommandFactoryMock.Object);

			var docs = new IClosedDocument[]
			{
				NullDocument.Instance
			};

			var command = factory.CreateCommand(docs);

			Assert.NotNull(command);
			Assert.IsType<FakeCommand2>(command);
			Assert.Equal((command as FakeCommand2).ClosedDocuments, docs);
		}
	}
}