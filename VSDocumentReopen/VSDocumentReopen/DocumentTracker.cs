using System.Collections.Generic;
using EnvDTE;

namespace VSDocumentReopen
{
	public sealed class DocumentTracker
	{
		private static readonly Stack<string> CloseDocuments;

		private DocumentTracker() { }

		static DocumentTracker()
		{
			Instance = new DocumentTracker();
			CloseDocuments = new Stack<string>();
		}

		public static DocumentTracker Instance { get; }

		public void Clear()
		{
			CloseDocuments.Clear();
		}

		public void AddClosed(Document document)
		{
			CloseDocuments.Push(document.FullName);
		}

		public string GetLastClosed()
		{
			if (CloseDocuments.Count > 0)
			{
				return CloseDocuments.Pop();
			}

			return string.Empty;
		}
	}
}