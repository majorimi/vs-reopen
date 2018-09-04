using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Repositories;
using VSDocumentReopen.Test.AssemblyFictures;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure
{
	public class JsonIHistoryToolWindowRepositoryFactoryTest : IAssemblyFixture<ConfigContext>
	{
		private readonly JsonIHistoryToolWindowRepositoryFactory _jsonHistoryRepositoryFactory;
		private readonly ConfigContext _configContext;

		public JsonIHistoryToolWindowRepositoryFactoryTest(ConfigContext configContext)
		{
			_jsonHistoryRepositoryFactory = new JsonIHistoryToolWindowRepositoryFactory(null);
			_configContext = configContext;
		}

		[Fact]
		public void ItShould_Handle_ValidObject()
		{
			_configContext.Configuration.SetupGet(g => g.PackageWorkingDirName).Returns("PackageWorkingDirName");
			_configContext.Configuration.SetupGet(g => g.ToolWindowSettingsFileName).Returns("ToolWindowSettingsFileName");

			var ret = _jsonHistoryRepositoryFactory.Create();

			Assert.NotNull(ret);
			Assert.IsType<JsonHistoryToolWindowRepository>(ret);
		}
	}
}