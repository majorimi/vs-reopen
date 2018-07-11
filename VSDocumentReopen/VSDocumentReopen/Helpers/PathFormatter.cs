using System.Collections.Generic;
using System.IO;

namespace VSDocumentReopen.Helpers
{
	public static class PathFormatter
	{
		public static string ShrinkPath(string absolutePath, int limit, string spacer = "…")
		{
			if (string.IsNullOrWhiteSpace(absolutePath))
			{
				return string.Empty;
			}
			if (absolutePath.Length <= limit)
			{
				return absolutePath;
			}

			var parts = new List<string>();

			var fi = new FileInfo(absolutePath);
			string drive = Path.GetPathRoot(fi.FullName);


			parts.Add(drive.TrimEnd('\\'));
			parts.Add(spacer);
			parts.Add(fi.Name);

			var ret = string.Join("\\", parts);
			var dir = fi.Directory;

			while (ret.Length < limit && dir != null)
			{
				if (ret.Length + dir.Name.Length > limit)
				{
					break;
				}

				parts.Insert(2, dir.Name);

				dir = dir.Parent;
				ret = string.Join("\\", parts);
			}

			return ret;
		}
	}
}