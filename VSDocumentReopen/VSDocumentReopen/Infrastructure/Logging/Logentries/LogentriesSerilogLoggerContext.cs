namespace VSDocumentReopen.Infrastructure.Logging.Logentries
{
	public sealed class LogentriesSerilogLoggerContext : LoggerContext
	{
		private static readonly ILogger _logger;

		static LogentriesSerilogLoggerContext()
		{
			_logger = new LogentriesSerilogLogger();
		}

		public override ILogger Logger => _logger;
	}
}