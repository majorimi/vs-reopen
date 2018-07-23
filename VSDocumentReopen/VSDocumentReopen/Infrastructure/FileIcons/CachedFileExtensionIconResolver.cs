using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public class CachedFileExtensionIconResolver : IFileExtensionIconResolver
	{
		private const string NoIconKey = "";
		private static readonly Dictionary<string, Bitmap> _icons;

		private readonly IFileExtensionIconResolver _fileExtensionIconResolver;

		static CachedFileExtensionIconResolver()
		{
			_icons = new Dictionary<string, Bitmap>();
			_icons.Add(NoIconKey, Resources.FileError_16x);
		}

		public CachedFileExtensionIconResolver(IFileExtensionIconResolver fileExtensionIconResolver)
		{
			_fileExtensionIconResolver = fileExtensionIconResolver;
		}

		public Bitmap GetIcon(IClosedDocument document)
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
