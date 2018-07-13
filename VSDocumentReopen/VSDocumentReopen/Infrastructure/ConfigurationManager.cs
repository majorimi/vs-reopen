using System;
using System.IO;
using System.Reflection;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure
{
	public static class ConfigurationManager
	{
		private static Lazy<Configuration> _loadConfig;

		static ConfigurationManager()
		{
			_loadConfig = new Lazy<Configuration>(LoadConfig);
		}

		public static Configuration Config => _loadConfig.Value;

		private static Configuration LoadConfig()
		{
			var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var config = Path.Combine(workingDir, "appConfig.json");

			var json = File.ReadAllText(config);
			return new ServiceStackJsonSerializer().Deserialize<Configuration>(json);
		}
	}
}