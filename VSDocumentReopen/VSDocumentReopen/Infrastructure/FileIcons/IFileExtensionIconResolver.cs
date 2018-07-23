using System.Drawing;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public interface IFileExtensionIconResolver
	{
		Bitmap GetIcon(IClosedDocument document);
	}

	public class VisualStudioFileExtensionIconResolver : IFileExtensionIconResolver
	{
		public Bitmap GetIcon(IClosedDocument document)
		{
			//TODO: implement somehow...
			//https://stackoverflow.com/questions/41296478/using-built-in-icons-in-visual-studio-extension
			//https://github.com/madskristensen/FileIcons

			return null; //https://www.mztools.com/articles/2014/MZ2014010.aspx
		}
	}
}
