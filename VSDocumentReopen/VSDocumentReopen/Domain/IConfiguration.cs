namespace VSDocumentReopen.Domain
{
	public interface IConfiguration
	{
		string HistoryFileName { get; }
		string ToolWindowSettingsFileName { get; }
		int MaxAllowedHistoryItems { get; }
        int MaxAllowedHistoryItemAgeInDays { get; }
        int MaxNumberOfHistoryItemsOnMenu { get; }
		string PackageWorkingDirName { get; }
		string ReopenCommandBinding { get; }
		string RemoveCommandBinding { get; }
		string ShowMoreCommandBinding { get; }
		string VSTempFolderName { get; }
	}
}