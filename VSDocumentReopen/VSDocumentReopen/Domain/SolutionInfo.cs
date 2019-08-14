using System.Diagnostics;

namespace VSDocumentReopen.Domain
{
	[DebuggerDisplay("SolutionInfo: {Name} - {FullPath}")]
	public class SolutionInfo
	{
		public string FullPath { get; }
		public string Name { get; }

		public SolutionInfo(string fullPath, string name)
		{
			FullPath = fullPath;
			Name = name;
		}

		public void Deconstruct(out string fullPath, out string name)
		{
			name = Name;
			fullPath = FullPath;
		}
	}
}