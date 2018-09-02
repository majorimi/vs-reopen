using Moq;
using System.ComponentModel;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Repositories;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure
{
	public class JsonHistoryRepositoryFactoryTest
	{
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly JsonHistoryRepositoryFactory _jsonHistoryRepositoryFactory;
		private readonly Mock<ConfigurationManager> _configurationManagerMock;

		public JsonHistoryRepositoryFactoryTest()
		{
			_configurationMock = new Mock<IConfiguration>();
			_configurationManagerMock = new Mock<ConfigurationManager>();
			_configurationManagerMock.SetupGet(g => g.Config).Returns(_configurationMock.Object);

			ConfigurationManager.Current = _configurationManagerMock.Object;

			_jsonHistoryRepositoryFactory = new JsonHistoryRepositoryFactory(null);
		}

		[Fact]
		public void ItShould_Handle_Null()
		{
			Assert.Throws<InvalidEnumArgumentException>(() =>
				_jsonHistoryRepositoryFactory.CreateHistoryRepository(null));
		}

		[Fact]
		public void ItShould_Handle_ValidObject()
		{
			_configurationMock.SetupGet(g => g.VSTempFolderName).Returns("VSTempFolderName");
			_configurationMock.SetupGet(g => g.PackageWorkingDirName).Returns("PackageWorkingDirName");
			_configurationMock.SetupGet(g => g.HistoryFileName).Returns("HistoryFileName");

			var ret = _jsonHistoryRepositoryFactory.CreateHistoryRepository(new SolutionInfo("c:\\", "test"));

			Assert.NotNull(ret);
			Assert.IsType<JsonHistoryRepository>(ret);
		}
	}
}