using System;

namespace VSDocumentReopen.Documents
{
	internal interface IClosedDocument
	{
		DateTime ClosedAt { get; }
		string FullName { get; }
		string Kind { get; }
		string Language { get; }
		string Name { get; }

		bool IsValid();
	}
}