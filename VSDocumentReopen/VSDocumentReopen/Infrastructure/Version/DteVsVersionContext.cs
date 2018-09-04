using EnvDTE;

namespace VSDocumentReopen.Infrastructure.Version
{
	public sealed class DteVsVersionContext : VsVersionContext
	{
		private readonly IVsVersionProvider _dteVsVersionProvider;

		public DteVsVersionContext(_DTE dte)
		{
			_dteVsVersionProvider = new VsDteVersionProvider(dte);
		}

		public override IVsVersionProvider VersionProvider
		{
			get => _dteVsVersionProvider;
		}
	}
}
