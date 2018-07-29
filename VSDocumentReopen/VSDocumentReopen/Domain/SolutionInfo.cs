namespace VSDocumentReopen.Domain
{
	public class SolutionInfo
	{
		public string FullPath { get; }
		public string Name { get; }

		public SolutionInfo(string fullPath, string name)
		{
			FullPath = fullPath;
			Name = name;
		}

		public void Deconstruct(out string name, out string fullPath)
		{
			name = Name;
			fullPath = FullPath;
		}
	}
}