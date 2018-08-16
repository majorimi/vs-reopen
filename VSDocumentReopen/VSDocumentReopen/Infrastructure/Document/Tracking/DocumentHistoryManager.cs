using System;
using System.Collections.Generic;
using System.Linq;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.Document.Tracking
{
	public sealed class DocumentHistoryManager : IDocumentHistoryManager
	{
		public event EventHandler HistoryChanged;

		private readonly List<IClosedDocument> ClosedDocuments;

		public int Count => ClosedDocuments.Count;

		public DocumentHistoryManager()
		{
			ClosedDocuments = new List<IClosedDocument>();
		}

		public void Clear()
		{
			ClosedDocuments.Clear();
			OnHistoryChanged();
		}

		public void Add(IClosedDocument document)
		{
			if(document == null)
			{
				return;
			}

			ClosedDocuments.Insert(0, document);
			OnHistoryChanged();
		}

		public IClosedDocument RemoveLast()
		{
			if (ClosedDocuments.Count > 0)
			{
				var ret = ClosedDocuments[0];
				ClosedDocuments.RemoveAt(0);
				OnHistoryChanged();

				return ret;
			}

			return NullDocument.Instance;
		}

		public IEnumerable<IClosedDocument> Get(int number) => ClosedDocuments.Take(number).ToArray();

		public IEnumerable<IClosedDocument> GetAll()
		{
			return ClosedDocuments.ToArray();
		}

		public void Remove(IClosedDocument closedDocument)
		{
			if (ClosedDocuments.Remove(closedDocument))
			{
				OnHistoryChanged();
			}
		}

		public void Remove(IEnumerable<IClosedDocument> closedDocuments)
		{
			bool removed = false;
			foreach (var item in closedDocuments)
			{
				if (ClosedDocuments.Remove(item))
				{
					removed = true;
				}
			}

			if(removed)
			{
				OnHistoryChanged();
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
				ClosedDocuments.Insert(0, document);
			}

			OnHistoryChanged();
		}

		private void OnHistoryChanged()
		{
			HistoryChanged?.Invoke(this, new EventArgs());
		}
	}
}