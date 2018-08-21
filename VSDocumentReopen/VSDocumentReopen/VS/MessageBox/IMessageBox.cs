namespace VSDocumentReopen.VS.MessageBox
{
	public interface IMessageBox
	{
		void ShowInfo(string title, string message);
		void ShowError(string title, string message);
	}
}