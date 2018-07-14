namespace VSDocumentReopen.Domain
{
	public sealed class Configuration
	{
		public string VSTempFolderName { get; set; }

		public string PackageWorkingDirName { get; set; }

		public string HistoryFileName { get; set; }

		public int MaxNumberOfHistoryItemsOnMenu { get; set; }

		public int MaxAllowedHistoryItems { get; set; }
	}
}