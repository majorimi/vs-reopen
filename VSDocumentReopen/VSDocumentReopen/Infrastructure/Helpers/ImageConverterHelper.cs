using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VSDocumentReopen.Infrastructure.Helpers
{
	public class ImageConverterHelper
	{
		public static byte[] BitmapToByteArray(Bitmap bmp, ImageFormat format)
		{
			if (bmp == null)
			{
				return null;
			}

			using (var ms = new MemoryStream())
			{
				bmp.Save(ms, format);
				return ms.ToArray();
			}
		}

		public static Bitmap ByteArrayToBitmap(byte[] bytes)
		{
			using (var ms = new MemoryStream(bytes))
			{
				var _bmp = Bitmap.FromStream(ms) as Bitmap;
				return _bmp;
			}
		}
	}
}