using Serilog.Core;
using Serilog.Events;
using VSDocumentReopen.Infrastructure.Version;

namespace VSDocumentReopen.Infrastructure.Logging.Logentries
{
	public class VisualStudioVersionEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			if (logEvent != null)
			{
				logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Visual Studio Version",
					VsVersionContext.Current.VersionProvider.GetVersion(),
					false));
			}
		}
	}
}
