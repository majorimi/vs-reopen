using Moq;
using System;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure;

namespace VSDocumentReopen.Test.AssemblyFictures
{
	public class ConfigContext : IDisposable
	{
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly Mock<ConfigurationManager> _configurationManagerMock;

		public Mock<IConfiguration> Configuration => _configurationMock;

		public ConfigContext()
		{
			_configurationMock = new Mock<IConfiguration>();
			_configurationManagerMock = new Mock<ConfigurationManager>();
			_configurationManagerMock.SetupGet(g => g.Config).Returns(_configurationMock.Object);

			ConfigurationManager.Current = _configurationManagerMock.Object;
		}

		public void Dispose()
		{
			ConfigurationManager.Current = null;
		}
	}
}