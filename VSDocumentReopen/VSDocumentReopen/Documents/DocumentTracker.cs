using System;
using System.Collections.Generic;
using EnvDTE;

namespace VSDocumentReopen.Documents
{
	internal sealed class DocumentTracker
	{
		private static readonly Stack<IClosedDocument> CloseDocuments;

		private DocumentTracker() { }

		static DocumentTracker()
		{
			Instance = new DocumentTracker();
			CloseDocuments = new Stack<IClosedDocument>();
		}

		public static DocumentTracker Instance { get; }

		public void Clear()
		{
			CloseDocuments.Clear();
		}

		public void AddClosed(Document document)
		{
			CloseDocuments.Push(new ClosedDocument()
			{
				FullName = document.FullName,
				Name = document.Name,
				Kind = document.Kind,
				Language = document.Language,
				ClosedAt = DateTime.Now,
			});
		}

		public IClosedDocument GetLastClosed()
		{
			if (CloseDocuments.Count > 0)
			{
				return CloseDocuments.Pop();
			}

			return NullDocument.Instance;
		}

		public IEnumerable<IClosedDocument> GetAll()
		{
			var ret = CloseDocuments.ToArray();
			return ret;
		}
	}
}