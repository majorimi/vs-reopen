using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Reflection;

namespace VSDocumentReopen.Infrastructure.Logging.Logentries
{
	public class ExtensionVersionEnricher : ILogEventEnricher
	{
		private readonly Lazy<string> _versionReader;

		public ExtensionVersionEnricher()
		{
			_versionReader = new Lazy<string>(GetPackageVersion, true);
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			if (logEvent != null)
			{
				logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("VSIX Version",
					_versionReader.Value,
					false));
			}
		}

		private string GetPackageVersion()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			return fvi.FileVersion;
		}
	}
}
