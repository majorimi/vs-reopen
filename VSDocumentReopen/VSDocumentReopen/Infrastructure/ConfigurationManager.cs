using System;
using System.IO;
using System.Reflection;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure
{
	public static class ConfigurationManager
	{
		private const string ConfigFileName = "appConfig.json";

		private static Lazy<IConfiguration> _loadConfig;

		static ConfigurationManager()
		{
			_loadConfig = new Lazy<IConfiguration>(LoadConfig);
		}

		public static IConfiguration Config => _loadConfig.Value;

		private static IConfiguration LoadConfig()
		{
			var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var config = Path.Combine(workingDir, ConfigFileName);

			var json = File.ReadAllText(config);
			return new ServiceStackJsonSerializer().Deserialize<Configuration>(json);
		}
	}
}