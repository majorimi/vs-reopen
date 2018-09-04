using EnvDTE;

namespace VSDocumentReopen.Infrastructure.Version
{
	public sealed class VsDteVersionContext : VsVersionContext
	{
		private readonly IVsVersionProvider _dteVsVersionProvider;

		public VsDteVersionContext(_DTE dte)
		{
			_dteVsVersionProvider = new VsDteVersionProvider(dte);
		}

		public override IVsVersionProvider VersionProvider => _dteVsVersionProvider;
	}
}