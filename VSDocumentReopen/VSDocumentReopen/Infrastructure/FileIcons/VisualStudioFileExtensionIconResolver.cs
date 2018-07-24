using Microsoft.VisualStudio.Shell.Interop;
using System.Drawing;
using VSDocumentReopen.Domain.Documents;
using GelUtilities = Microsoft.Internal.VisualStudio.PlatformUI.Utilities;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public class VisualStudioFileExtensionIconResolver : IFileExtensionIconResolver
	{
		private readonly IVsImageService2 _vsImageService2;

		public VisualStudioFileExtensionIconResolver(IVsImageService2 vsImageService2)
		{
			_vsImageService2 = vsImageService2;
		}

		//https://msdn.microsoft.com/en-us/library/mt628927.aspx?f=255&MSPPError=-2147217396
		public Icon GetIcon(IClosedDocument document)
		{
			IVsUIObject uIObj = _vsImageService2.GetIconForFile(document.Name, __VSUIDATAFORMAT.VSDF_WINFORMS);
			Icon icon = (Icon)GelUtilities.GetObjectData(uIObj);

			return icon;
		}
	}
}