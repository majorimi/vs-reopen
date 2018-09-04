using Moq;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Repositories;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure
{
	public class JsonIHistoryToolWindowRepositoryFactoryTest
	{
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly JsonIHistoryToolWindowRepositoryFactory _jsonHistoryRepositoryFactory;
		private readonly Mock<ConfigurationManager> _configurationManagerMock;

		public JsonIHistoryToolWindowRepositoryFactoryTest()
		{
			_configurationMock = new Mock<IConfiguration>();
			_configurationManagerMock = new Mock<ConfigurationManager>();
			_configurationManagerMock.SetupGet(g => g.Config).Returns(_configurationMock.Object);

			ConfigurationManager.Current = _configurationManagerMock.Object;

			_jsonHistoryRepositoryFactory = new JsonIHistoryToolWindowRepositoryFactory(null);
		}

		[Fact]
		public void ItShould_Handle_ValidObject()
		{
			_configurationMock.SetupGet(g => g.PackageWorkingDirName).Returns("PackageWorkingDirName");
			_configurationMock.SetupGet(g => g.ToolWindowSettingsFileName).Returns("ToolWindowSettingsFileName");

			var ret = _jsonHistoryRepositoryFactory.Create();

			Assert.NotNull(ret);
			Assert.IsType<JsonHistoryToolWindowRepository>(ret);
		}
	}
}