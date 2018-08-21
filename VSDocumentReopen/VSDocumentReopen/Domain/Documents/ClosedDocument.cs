using System;
using System.IO;

namespace VSDocumentReopen.Domain.Documents
{
	public sealed class ClosedDocument : IClosedDocument
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
			return $"ClosedDocument FullName: {FullName}, ClosedAt: {ClosedAt}";
		}
	}
}