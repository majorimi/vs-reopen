using System.Threading.Tasks;
using Moq;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.VS.ToolWindows;
using Xunit;

namespace VSDocumentReopen.Test.VS.ToolWindows
{
	public class ClosedDocumentsHistoryTest
	{
		private readonly Mock<IDocumentHistoryQueries> _docuemntQueriesMock;

		public ClosedDocumentsHistoryTest()
		{
			_docuemntQueriesMock = new Mock<IDocumentHistoryQueries>();
		}

		[StaFact]
		public async Task ItShould_Initialize()
		{
			await ClosedDocumentsHistory.InitializeAsync(_docuemntQueriesMock.Object, null, null, null, null, null);
			Assert.NotNull(ClosedDocumentsHistory.ContentWindow);
		}
	}
}