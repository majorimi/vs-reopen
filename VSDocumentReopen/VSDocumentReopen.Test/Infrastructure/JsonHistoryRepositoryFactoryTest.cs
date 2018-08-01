using System.ComponentModel;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure
{
	public class JsonHistoryRepositoryFactoryTest
	{
		private readonly JsonHistoryRepositoryFactory _jsonHistoryRepositoryFactory;

		public JsonHistoryRepositoryFactoryTest()
		{
			_jsonHistoryRepositoryFactory = new JsonHistoryRepositoryFactory(null);
		}

		[Fact]
		public void ItShould_Handle_Null()
		{
			Assert.Throws<InvalidEnumArgumentException>(() =>
				_jsonHistoryRepositoryFactory.CreateHistoryRepository(null));
		}


		//[Fact]
		//public void ItShould_Handle_ValidObject()
		//{
		//	var ret = _jsonHistoryRepositoryFactory.CreateHistoryRepository(new SolutionInfo("c:\\", "test"));

		//	Assert.NotNull(ret);
		//}
	}
}