using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using EnvDTE;

namespace VSDocumentReopen.Documents
{
	internal sealed class DocumentTracker : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

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
			OnPropertyChanged();
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

			OnPropertyChanged();
		}

		public IClosedDocument GetLastClosed()
		{
			if (CloseDocuments.Count > 0)
			{
				var ret = CloseDocuments.Pop();
				OnPropertyChanged();

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

		public void Initialize(IEnumerable<IClosedDocument> closedDocuments, bool reverse = true)
		{
			Clear();

			closedDocuments = closedDocuments.Reverse();
			foreach (var document in closedDocuments)
			{
				CloseDocuments.Push(document);
			}

			OnPropertyChanged();
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}