using System;
using System.IO;

namespace VSDocumentReopen.Documents
{
	internal sealed class ClosedDocument : IClosedDocument
	{
		public string FullName { get; set; }

		public string Name { get; set; }

		public string Kind { get; set; }

		public string Language { get; set; }

		public DateTime ClosedAt { get; set; }

		public bool IsValid()
		{
			return File.Exists(FullName);
		}

		public override string ToString()
		{
			return $"FullName: {FullName}, ClosedAt: {ClosedAt}";
		}
	}
}