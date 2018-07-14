using System;

namespace VSDocumentReopen.Domain.Documents
{
	public interface IClosedDocument
	{
		DateTime ClosedAt { get; }
		string FullName { get; }
		string Kind { get; }
		string Language { get; }
		string Name { get; }

		bool IsValid();
	}
}