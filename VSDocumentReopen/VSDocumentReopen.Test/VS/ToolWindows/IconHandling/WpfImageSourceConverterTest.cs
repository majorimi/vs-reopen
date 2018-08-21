using System.Drawing;
using VSDocumentReopen.VS.ToolWindows.IconHandling;
using Xunit;

namespace VSDocumentReopen.Test.VS.ToolWindows.IconHandling
{
	public class WpfImageSourceConverterTest
	{
		[Fact]
		public void CreateBitmapSource_Should_Handle_Null_Bytes()
		{
			var ret = WpfImageSourceConverter.CreateBitmapSource(null as byte[]);

			Assert.Null(ret);
		}

		[Fact]
		public void CreateBitmapSource_Should_Handle_Empty_Bytes()
		{
			var ret = WpfImageSourceConverter.CreateBitmapSource(new byte[0]);

			Assert.Null(ret);
		}

		[Fact]
		public void CreateBitmapSource_Should_Handle_Bytes()
		{
			var converter = new ImageConverter();
			var bytes = (byte[])converter.ConvertTo(Resources.OpenFile_16x, typeof(byte[]));

			var ret = WpfImageSourceConverter.CreateBitmapSource(bytes);

			Assert.NotNull(ret);
			Assert.Equal(16, ret.Width);
			Assert.Equal(16, ret.Height);
		}

		[Fact]
		public void CreateBitmapSource_Should_Handle_Null_Bitmap()
		{
			var ret = WpfImageSourceConverter.CreateBitmapSource(null as Bitmap);

			Assert.Null(ret);
		}

		[Fact]
		public void CreateBitmapSource_Should_Handle_Bitmap()
		{
			var ret = WpfImageSourceConverter.CreateBitmapSource(Resources.OpenFile_16x);

			Assert.NotNull(ret);
			Assert.Equal(16, ret.Width);
			Assert.Equal(16, ret.Height);
		}
	}
}