using System;
using System.Collections.Generic;
using System.Linq;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Document.Tracking
{
	public class DocumentHistoryManagerTest
	{
		private readonly DocumentHistoryManager _documentHistoryManager;
		private bool _historyChanged;

		public DocumentHistoryManagerTest()
		{
			_documentHistoryManager = new DocumentHistoryManager();
			_historyChanged = false;
			_documentHistoryManager.HistoryChanged += _documentHistoryManager_HistoryChanged;
		}

		private void _documentHistoryManager_HistoryChanged(object sender, EventArgs e) => _historyChanged = true;

		[Fact]
		public void ItShould_Handle_No_Event_Subscribers()
		{
			_documentHistoryManager.HistoryChanged -= _documentHistoryManager_HistoryChanged;

			_documentHistoryManager.Add(NullDocument.Instance);

			Assert.False(_historyChanged);
		}

		[Fact]
		public void ItShould_Be_Empty_ByDefault()
		{
			Assert.Equal(0, _documentHistoryManager.Count);
		}

		[Fact]
		public void ItShould_Not_Add_Null()
		{
			_documentHistoryManager.Add(null);

			Assert.Equal(0, _documentHistoryManager.Count);
		}

		[Fact]
		public void ItShould_Add_Document()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs" });
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs" });

			var all = _documentHistoryManager.GetAll();

			Assert.True(_historyChanged);
			Assert.Equal(2, _documentHistoryManager.Count);
			Assert.Equal("c:\\test.cs", all.ElementAt(0).FullName);
			Assert.Equal(DateTime.MinValue, all.ElementAt(0).ClosedAt);
			Assert.Equal(NullDocument.Instance, all.ElementAt(1));
			Assert.Equal(DateTime.MinValue, all.ElementAt(1).ClosedAt);
		}

		[Fact]
		public void ItShould_Clear_All_Documents_Raise_Event()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Clear();

			Assert.True(_historyChanged);
			Assert.Equal(0, _documentHistoryManager.Count);
		}

		[Fact]
		public void ItShould_Never_Return_Null_From_RemoveLast_Document()
		{
			var last = _documentHistoryManager.RemoveLast();

			Assert.NotNull(last);
			Assert.Equal(last, NullDocument.Instance);
			Assert.Equal(0, _documentHistoryManager.Count);
		}

		[Fact]
		public void ItShould_RemoveLast_Document()
		{
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs" });
			_documentHistoryManager.Add(NullDocument.Instance);

			var last = _documentHistoryManager.RemoveLast();
			var all = _documentHistoryManager.GetAll();

			Assert.Equal(1, _documentHistoryManager.Count);
			Assert.Equal(NullDocument.Instance, last);
			Assert.Single(all);
			Assert.Equal("c:\\test.cs", all.First().FullName);
		}

		[Fact]
		public void ItShould_Get_NumberOf_Document()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test3.cs", ClosedAt = DateTime.Now });

			var result = _documentHistoryManager.Get(2);

			Assert.Equal(4, _documentHistoryManager.Count);
			Assert.Equal(2, result.Count());
			Assert.Equal("c:\\test3.cs", result.ElementAt(0).FullName);
			Assert.Equal("c:\\test2.cs", result.ElementAt(1).FullName);
		}

		[Fact]
		public void ItShould_GetAll_Document()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test3.cs", ClosedAt = DateTime.Now });

			var all = _documentHistoryManager.GetAll();

			Assert.Equal(4, _documentHistoryManager.Count);
			Assert.Equal(4, all.Count());
			Assert.Equal("c:\\test3.cs", all.ElementAt(0).FullName);
			Assert.Equal("c:\\test2.cs", all.ElementAt(1).FullName);
			Assert.Equal("c:\\test.cs", all.ElementAt(2).FullName);
			Assert.Equal(NullDocument.Instance, all.ElementAt(3));
		}

		[Fact]
		public void ItShould_Remove_Document()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test3.cs", ClosedAt = DateTime.Now });

			IClosedDocument doc = _documentHistoryManager.Get(2).Last(); //test2.cs
			_documentHistoryManager.Remove(doc);

			var all = _documentHistoryManager.GetAll();

			Assert.True(_historyChanged);
			Assert.Equal(3, _documentHistoryManager.Count);
			Assert.Equal(3, all.Count());
			Assert.Equal("c:\\test3.cs", all.ElementAt(0).FullName);
			Assert.Equal("c:\\test.cs", all.ElementAt(1).FullName);
			Assert.Equal(NullDocument.Instance, all.ElementAt(2));
		}

		[Fact]
		public void ItShould_Remove_Multiple_Document()
		{
			_documentHistoryManager.Add(NullDocument.Instance);
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test.cs", ClosedAt = DateTime.Now});
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = DateTime.Now });
			_documentHistoryManager.Add(new ClosedDocument() { FullName = "c:\\test3.cs", ClosedAt = DateTime.Now });

			var docs = _documentHistoryManager.Get(2);
			_documentHistoryManager.Remove(docs);

			var all2 = _documentHistoryManager.GetAll();

			Assert.True(_historyChanged);
			Assert.Equal(2, _documentHistoryManager.Count);
			Assert.Equal(2, all2.Count());
			Assert.Equal("c:\\test.cs", all2.ElementAt(0).FullName);
			Assert.Equal(NullDocument.Instance, all2.ElementAt(1));
		}

		[Fact]
		public void ItShould_Handle_Null_Initialize()
		{
			_documentHistoryManager.Initialize(null);

			Assert.True(_historyChanged);
			Assert.Equal(0, _documentHistoryManager.Count);
		}

		[Fact]
		public void ItShould_Initialize_History()
		{
			var docs = new List<IClosedDocument>()
			{
				NullDocument.Instance,
				new ClosedDocument() { FullName = "c:\\test.cs", ClosedAt = new DateTime(2017, 08, 05) },
				NullDocument.Instance,
				new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = new DateTime(2017, 08, 06) },
				new ClosedDocument() { FullName = "c:\\test2.cs", ClosedAt = new DateTime(2017, 08, 07) },
			};

			_documentHistoryManager.Initialize(docs);
			var all = _documentHistoryManager.GetAll();

			Assert.True(_historyChanged);
			Assert.Equal(3, _documentHistoryManager.Count);
			Assert.Equal("c:\\test2.cs", all.ElementAt(0).FullName);
			Assert.Equal(new DateTime(2017, 08, 07), all.ElementAt(0).ClosedAt);
			Assert.Equal("c:\\test.cs", all.ElementAt(1).FullName);
			Assert.Equal(new DateTime(2017, 08, 05), all.ElementAt(1).ClosedAt);
			Assert.Equal(NullDocument.Instance, all.ElementAt(2));
		}
	}
}