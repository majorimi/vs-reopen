namespace VSDocumentReopen.Infrastructure.Logging
{
	public abstract class LoggerContext
	{
		private static readonly LoggerContext Default = new DefaultLoggerContext();

		private static LoggerContext _current;

		public static LoggerContext Current
		{
			get => _current ?? Default;
			set => _current = value;
		}

		public abstract ILogger Logger { get; }
	}
}