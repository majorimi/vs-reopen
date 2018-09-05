using System.Threading.Tasks;
using Moq;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.VS.ToolWindows;
using Xunit;

namespace VSDocumentReopen.Test.VS.ToolWindows
{
	public class ClosedDocumentsHistoryTest
	{
		private readonly Mock<IDocumentHistoryQueries> _docuemntQueriesMock;
		private readonly Mock<IHistoryToolWindowRepositoryFactory> _historyToolWindowRepositoryFactoryMock;

		public ClosedDocumentsHistoryTest()
		{
			_docuemntQueriesMock = new Mock<IDocumentHistoryQueries>();
			_historyToolWindowRepositoryFactoryMock = new Mock<IHistoryToolWindowRepositoryFactory>();
		}

		[StaFact]
		public async Task ItShould_Initialize()
		{
			await ClosedDocumentsHistory.InitializeAsync(_docuemntQueriesMock.Object,
				null,
				null,
				null,
				null,
				null,
				_historyToolWindowRepositoryFactoryMock.Object);

			Assert.NotNull(ClosedDocumentsHistory.ContentWindow);
		}
	}
}