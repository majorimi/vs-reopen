using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace VSDocumentReopen.Infrastructure.Helpers
{
	public class WpfImageSourceConverter
	{
		public static BitmapSource CreateBitmapSource(byte[] img)
		{
			var image = ImageConverterHelper.ByteArrayToBitmap(img) as Bitmap;

			return CreateBitmapSource(image);
		}

		public static BitmapSource CreateBitmapSource(Bitmap img)
		{
			var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
				img.GetHbitmap(),
				IntPtr.Zero,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions());

			return bitmapSource;
		}
	}
}