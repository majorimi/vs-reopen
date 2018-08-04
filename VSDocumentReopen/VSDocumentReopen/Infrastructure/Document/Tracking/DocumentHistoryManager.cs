using System;
using System.Collections.Generic;
using System.Linq;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Document.Tracking
{
	public sealed class DocumentHistoryManager : IDocumentHistoryManager
	{
		public event EventHandler HistoryChanged;

		private readonly Stack<IClosedDocument> CloseDocuments;

		public int Count => CloseDocuments.Count;

		public DocumentHistoryManager()
		{
			CloseDocuments = new Stack<IClosedDocument>();
		}

		public void Clear()
		{
			CloseDocuments.Clear();
			OnHistoryChanged();
		}

		public void Add(IClosedDocument document)
		{
			if(document == null)
			{
				return;
			}

			CloseDocuments.Push(document);
			OnHistoryChanged();
		}

		public IClosedDocument RemoveLast()
		{
			if (CloseDocuments.Count > 0)
			{
				var ret = CloseDocuments.Pop();
				OnHistoryChanged();

				return ret;
			}

			return NullDocument.Instance;
		}

		public IEnumerable<IClosedDocument> Get(int number) => CloseDocuments.ToList().Take(number);

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

		public void Remove(IEnumerable<IClosedDocument> closedDocuments)
		{
			var items = GetAll().ToList();

			bool removed = false;
			foreach (var item in closedDocuments)
			{
				if (items.Remove(item))
				{
					removed = true;
				}
			}

			if(removed)
			{
				Initialize(items);
			}
		}

		public void Initialize(IEnumerable<IClosedDocument> closedDocuments)
		{
			Clear();

			if (closedDocuments is null)
			{
				return;
			}

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