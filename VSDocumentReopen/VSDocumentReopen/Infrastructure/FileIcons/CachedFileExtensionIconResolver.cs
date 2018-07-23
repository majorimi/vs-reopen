using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public class CachedFileExtensionIconResolver : IFileExtensionIconResolver
	{
		private const string NoIconKey = "";
		private static readonly Dictionary<string, Icon> _icons;

		private readonly IFileExtensionIconResolver _fileExtensionIconResolver;

		static CachedFileExtensionIconResolver()
		{
			_icons = new Dictionary<string, Icon>();
			_icons.Add(NoIconKey, Icon.FromHandle(Resources.FileError_16x.GetHicon()));
		}

		public CachedFileExtensionIconResolver(IFileExtensionIconResolver fileExtensionIconResolver)
		{
			_fileExtensionIconResolver = fileExtensionIconResolver;
		}

		public Icon GetIcon(IClosedDocument document)
		{
			var extension = Path.GetExtension(document.FullName).ToLower();

			if(!_icons.ContainsKey(extension))
			{
				var icon = _fileExtensionIconResolver.GetIcon(document);
				if(icon is null)
				{
					return _icons[NoIconKey];
				}

				_icons.Add(extension, icon);
				return icon;
			}

			return _icons[extension];
		}
	}
}
