using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public class CachedFileExtensionIconResolver : IFileExtensionIconResolver
	{
		private const string NoIconKey = "";
		private static readonly Dictionary<string, Icon> Icons;

		private readonly IFileExtensionIconResolver _fileExtensionIconResolver;

		static CachedFileExtensionIconResolver()
		{
			Icons = new Dictionary<string, Icon>();
			Icons.Add(NoIconKey, Icon.FromHandle(Resources.FileError_16x.GetHicon()));
		}

		public CachedFileExtensionIconResolver(IFileExtensionIconResolver fileExtensionIconResolver)
		{
			_fileExtensionIconResolver = fileExtensionIconResolver;
		}

		public Icon GetIcon(IClosedDocument document)
		{
			var extension = Path.GetExtension(document?.FullName)?.ToLower() ?? NoIconKey;

			if(!Icons.ContainsKey(extension))
			{
				var icon = _fileExtensionIconResolver.GetIcon(document);
				if(icon is null)
				{
					return Icons[NoIconKey];
				}

				Icons.Add(extension, icon);
				return icon;
			}

			return Icons[extension];
		}
	}
}
