using System.Collections.Generic;

namespace VSDocumentReopen.Domain.Documents
{
	public class ClosedDocumentComparer : IEqualityComparer<IClosedDocument>
	{
		public bool Equals(IClosedDocument x, IClosedDocument y)
		{
			return x?.FullName == y?.FullName;
		}

		public int GetHashCode(IClosedDocument obj)
		{
			return obj?.FullName?.GetHashCode() ?? 0;
		}
	}
}