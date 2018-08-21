using VSDocumentReopen.Domain;

namespace VSDocumentReopen.Infrastructure
{
	public abstract class ConfigurationManager
	{
		private static readonly ConfigurationManager Default = new DefaultConfigurationManager();

		private static ConfigurationManager _current;

		public static ConfigurationManager Current
		{
			get => _current ?? (_current = Default);
			set => _current = value ?? Default;
		}

		public abstract IConfiguration Config { get; }
	}
}