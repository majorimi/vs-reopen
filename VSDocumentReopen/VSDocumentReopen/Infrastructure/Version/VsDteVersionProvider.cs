using EnvDTE;

namespace VSDocumentReopen.Infrastructure.Version
{
	public class VsDteVersionProvider : IVsVersionProvider
	{
		private readonly _DTE _dte;

		public VsDteVersionProvider(_DTE dte)
		{
			_dte = dte;
		}

		public string GetVersion()
		{
			return _dte?.Version;
		}
	}
}