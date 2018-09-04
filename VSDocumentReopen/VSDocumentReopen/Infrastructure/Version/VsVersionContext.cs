namespace VSDocumentReopen.Infrastructure.Version
{
	public abstract class VsVersionContext
	{
		public static VsVersionContext Current { get; set; }

		public abstract IVsVersionProvider VersionProvider { get; }
	}
}
