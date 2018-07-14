using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.ClosedDocument
{
	public sealed class DocumentHistory
	{
		public event EventHandler HistoryChanged;

		private static readonly Stack<IClosedDocument> CloseDocuments;

		private DocumentHistory() { }

		static DocumentHistory()
		{
			Instance = new DocumentHistory();
			CloseDocuments = new Stack<IClosedDocument>();
		}

		public static DocumentHistory Instance { get; }

		public void Clear()
		{
			CloseDocuments.Clear();
			OnHistoryChanged();
		}

		public void AddClosed(Document document)
		{
			if(document == null)
			{
				return;
			}

			CloseDocuments.Push(new Domain.Documents.ClosedDocument()
			{
				FullName = document.FullName,
				Name = document.Name,
				Kind = document.Kind,
				Language = document.Language,
				ClosedAt = DateTime.Now,
			});

			OnHistoryChanged();
		}

		public IClosedDocument GetLastClosed()
		{
			if (CloseDocuments.Count > 0)
			{
				var ret = CloseDocuments.Pop();
				OnHistoryChanged();

				return ret;
			}

			return NullDocument.Instance;
		}

		public IEnumerable<IClosedDocument> GetAll()
		{
			return CloseDocuments.ToArray();
		}

		public void Remove(IClosedDocument closedDocument)
		{
			var items = GetAll().ToList();
			if (items.Remove(closedDocument))
			{
				Initialize(items);
			}
		}

		public void Initialize(IEnumerable<IClosedDocument> closedDocuments)
		{
			Clear();

			closedDocuments = closedDocuments.OrderBy(x => x.ClosedAt);

			foreach (var document in closedDocuments)
			{
				CloseDocuments.Push(document);
			}

			OnHistoryChanged();
		}

		private void OnHistoryChanged()
		{
			HistoryChanged?.Invoke(this, new EventArgs());
		}
	}
}