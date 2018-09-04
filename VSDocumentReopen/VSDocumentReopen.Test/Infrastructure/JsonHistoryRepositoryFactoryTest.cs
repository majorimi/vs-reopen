using System.ComponentModel;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Repositories;
using VSDocumentReopen.Test.AssemblyFictures;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure
{
	public class JsonHistoryRepositoryFactoryTest : IAssemblyFixture<ConfigContext>
	{
		private readonly JsonHistoryRepositoryFactory _jsonHistoryRepositoryFactory;
		private readonly ConfigContext _configContext;

		public JsonHistoryRepositoryFactoryTest(ConfigContext configContext)
		{
			_jsonHistoryRepositoryFactory = new JsonHistoryRepositoryFactory(null);
			_configContext = configContext;
		}

		[Fact]
		public void ItShould_Handle_Null()
		{
			Assert.Throws<InvalidEnumArgumentException>(() => _jsonHistoryRepositoryFactory.CreateHistoryRepository(null));
		}

		[Fact]
		public void ItShould_Handle_ValidObject()
		{
			_configContext.Configuration.SetupGet(g => g.VSTempFolderName).Returns("VSTempFolderName");
			_configContext.Configuration.SetupGet(g => g.PackageWorkingDirName).Returns("PackageWorkingDirName");
			_configContext.Configuration.SetupGet(g => g.HistoryFileName).Returns("HistoryFileName");

			var ret = _jsonHistoryRepositoryFactory.CreateHistoryRepository(new SolutionInfo("c:\\", "test"));

			Assert.NotNull(ret);
			Assert.IsType<JsonHistoryRepository>(ret);
		}
	}
}