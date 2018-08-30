using System;
using System.Drawing.Imaging;
using VSDocumentReopen.Infrastructure.Helpers;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Helpers
{
	public class ImageConverterHelperTest
	{
		[Fact]
		public void BitmapToByteArray_Should_Handle_NullImage()
		{
			var ret = ImageConverterHelper.BitmapToByteArray(null, null);

			Assert.Null(ret);
		}

		[Fact]
		public void BitmapToByteArray_Should_Handle_NullImageFormat()
		{
			Assert.Throws<ArgumentNullException>(() => 
				ImageConverterHelper.BitmapToByteArray(Resources.ClearWindowContent_16x_Gray, null));
		}

		[Fact]
		public void BitmapToByteArray_Should_Handle_ValidObject()
		{
			var ret = ImageConverterHelper.BitmapToByteArray(Resources.ClearWindowContent_16x_Gray, ImageFormat.Png);

			Assert.NotNull(ret);
			Assert.Equal(210, ret.Length);
		}

		[Fact]
		public void ByteArrayToBitmap_Should_Handle_Null()
		{
			Assert.Throws<ArgumentNullException>(() =>
				ImageConverterHelper.ByteArrayToBitmap(null));
		}

		[Fact]
		public void ByteArrayToBitmap_Should_Handle_EmptyArray()
		{
			Assert.Throws<ArgumentException>(() =>
				ImageConverterHelper.ByteArrayToBitmap(new byte[0]));
		}

		[Fact]
		public void ByteArrayToBitmap_Should_Handle_ValidObject()
		{
			var bytes = ImageConverterHelper.BitmapToByteArray(Resources.ClearWindowContent_16x_Gray, ImageFormat.Png);
			var ret = ImageConverterHelper.ByteArrayToBitmap(bytes);

			Assert.NotNull(ret);
			Assert.Equal(16, ret.Width);
			Assert.Equal(16, ret.Height);
		}
	}
}