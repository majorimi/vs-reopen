namespace VSDocumentReopen.Infrastructure.Logging
{
	public sealed class DefaultLoggerContext : LoggerContext
	{
		public override ILogger Logger => NullLogger.Instance;
	}
}