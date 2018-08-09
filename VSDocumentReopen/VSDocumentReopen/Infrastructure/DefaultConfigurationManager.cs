using System;
using System.IO;
using System.Reflection;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure
{
	public class DefaultConfigurationManager : ConfigurationManager
	{
		private const string ConfigFileName = "appConfig.json";

		private Lazy<IConfiguration> _loadConfig;

		public DefaultConfigurationManager()
		{
			_loadConfig = new Lazy<IConfiguration>(LoadConfig);
		}

		public override IConfiguration Config => _loadConfig.Value;

		private static IConfiguration LoadConfig()
		{
			var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var config = Path.Combine(workingDir, ConfigFileName);

			var json = File.ReadAllText(config);
			return new ServiceStackJsonSerializer().Deserialize<Configuration>(json);
		}
	}
}