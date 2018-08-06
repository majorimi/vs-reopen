using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.VS.ToolWindows.IconHandling
{
	public class WpfImageSourceConverter
	{
		public static BitmapSource CreateBitmapSource(byte[] img)
		{
			if (img is null || img.Length == 0)
			{
				return null;
			}

			var image = ImageConverterHelper.ByteArrayToBitmap(img);

			return CreateBitmapSource(image);
		}

		public static BitmapSource CreateBitmapSource(Bitmap img)
		{
			if(img is null)
			{
				return null;
			}

			var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
				img.GetHbitmap(),
				IntPtr.Zero,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions());

			return bitmapSource;
		}
	}
}