namespace VSDocumentReopen.Domain
{
	public sealed class Configuration : IConfiguration
	{
		public string VSTempFolderName { get; set; }

		public string PackageWorkingDirName { get; set; }

		public string HistoryFileName { get; set; }

		public int MaxNumberOfHistoryItemsOnMenu { get; set; }

		public int MaxAllowedHistoryItems { get; set; }

        public int MaxAllowedHistoryItemAgeInDays { get; set; }

        public string ReopenCommandBinding { get; set; }

		public string RemoveCommandBinding { get; set; }

		public string ShowMoreCommandBinding { get; set; }

		public string ToolWindowSettingsFileName { get; set; }
    }
}