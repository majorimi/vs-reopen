using System;

namespace VSDocumentReopen.Documents
{
	internal sealed class NullDocument : IClosedDocument
	{
		internal static readonly NullDocument Instance; 

		static NullDocument()
		{
			Instance = new NullDocument();
		}

		public DateTime ClosedAt { get; }
		public string FullName { get;}
		public string Kind { get; }
		public string Language { get; }
		public string Name { get; }

		private NullDocument()
		{
			FullName = string.Empty;
			Kind = string.Empty;
			Language = string.Empty;
			Name = string.Empty;
			ClosedAt = DateTime.MinValue;
		}

		public bool IsValid() => false;
	}
}