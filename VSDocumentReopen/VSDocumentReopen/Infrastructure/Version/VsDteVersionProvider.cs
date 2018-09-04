using EnvDTE;

namespace VSDocumentReopen.Infrastructure.Version
{
	public class VsDteVersionProvider : IVsVersionProvider
	{
		private readonly _DTE _dTE;

		public VsDteVersionProvider(_DTE dTE)
		{
			_dTE = dTE;
		}

		public string GetVersion()
		{
			return _dTE?.Version;
		}
	}
}
